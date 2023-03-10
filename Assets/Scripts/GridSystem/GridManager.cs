using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public const GridLayer GizmoLayer = GridLayer.Default;


        private static GridManager _instance;
        [SerializeField] private Vector2 _worldSize = new(20, 20);
        [SerializeField] private Vector2 _gridOrigin = new(0, 0);
        [SerializeField] private float _cellSize = 0.5f;
        [Space] [SerializeField] private GridLayer _gizmoLayer = GridLayer.Default;
        [SerializeField] private bool _enableGizmo = true;
        private Camera _camera;
        private GridBase<GridItem> _defaultGridBase;
        private Dictionary<GridLayer, GridBase<GridItem>> _gridDict;
        private Vector3 _gridLeftBottom;

        private Vector2Int _size;

        public GridLayer gizmoLayer
        {
            get => _gizmoLayer;
            set => _gizmoLayer = value;
        }

        public Vector2 worldSize => _worldSize;
        public float cellSize => _cellSize;
        public float halfCellSize { get; private set; }

        public static GridManager instance
        {
            get
            {
                if (!_instance) SetupInstance();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);

            halfCellSize = _cellSize / 2;
            _size = new Vector2Int(Mathf.FloorToInt(_worldSize.x / _cellSize),
                Mathf.FloorToInt(_worldSize.y / _cellSize));
            _gridLeftBottom = new Vector3(_gridOrigin.x - _size.x * _cellSize / 2, 0,
                _gridOrigin.y - _size.y * _cellSize / 2);

            _gridDict = new Dictionary<GridLayer, GridBase<GridItem>>();
            _defaultGridBase = new GridBase<GridItem>(_size.x, _size.y, _cellSize, _gridLeftBottom);
            _gridDict.Add(GridLayer.Default, _defaultGridBase);
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<GridManager>();
            if (!_instance)
            {
                var gameObj = new GameObject();
                gameObj.name = nameof(GridManager);
                _instance = gameObj.AddComponent<GridManager>();
            }
        }

        // todo: this call can be cached
        public bool GetAvailability(GridLayer layer, Vector3 position, Vector2Int sizeOnGrid,
            out GridAvailability availability)
        {
            var canPlace = true;

            var offset = CalculateOffset(sizeOnGrid);
            var gridPosition = GetGridPositionWithOffset(position, offset);
            var startingGridPosition = gridPosition - (
                new Vector2Int(Mathf.FloorToInt(sizeOnGrid.x / 2.0f),
                    Mathf.FloorToInt(sizeOnGrid.y / 2.0f)) -
                Vector2Int.one + new Vector2Int(sizeOnGrid.x % 2, sizeOnGrid.y % 2));
            // var origin = GetOriginWithOffset(position, offset);

            if (startingGridPosition.x < 0) startingGridPosition.x = 0;
            if (startingGridPosition.y < 0) startingGridPosition.y = 0;
            if (startingGridPosition.x + sizeOnGrid.x > _size.x) startingGridPosition.x = _size.x - sizeOnGrid.x;
            if (startingGridPosition.y + sizeOnGrid.y > _size.y) startingGridPosition.y = _size.y - sizeOnGrid.y;

            var origin = GetOrigin(startingGridPosition, sizeOnGrid);

            var overlappedItems = new Dictionary<GridItem, List<Vector2Int>>();
            var itemArray = _gridDict[layer]
                .GetValueMultiple(startingGridPosition.x, startingGridPosition.y, sizeOnGrid.x, sizeOnGrid.y);
            for (var x = 0; x < sizeOnGrid.x; x++)
            for (var y = 0; y < sizeOnGrid.y; y++)
            {
                var item = itemArray[x, y];
                if (item)
                {
                    if (!overlappedItems.ContainsKey(item)) overlappedItems[item] = new List<Vector2Int>();

                    overlappedItems[item]
                        .Add(new Vector2Int(startingGridPosition.x + x, startingGridPosition.y + y));
                    canPlace = false;
                }
            }

            availability = new GridAvailability(layer, sizeOnGrid, origin, startingGridPosition,
                overlappedItems);

            return canPlace;
        }

        public bool PlaceIntoGrid(GridItem item, Vector3 worldPosition, out GridAvailability gridAvailability)
        {
            var canPlace = GetAvailability(item.layer, worldPosition, item.sizeOnGrid, out var availability);
            gridAvailability = availability;
            if (!canPlace) return false;
            _gridDict[item.layer]
                .SetValueMultiple(availability.startingCellPosition.x, availability.startingCellPosition.y,
                    item.sizeOnGrid.x, item.sizeOnGrid.y, item);
            item.transform.position = availability.origin;
            return true;
        }

        public void RemoveFromGrid(GridLayer layer, Vector2Int startingCell, Vector2Int sizeOnGrid)
        {
            _gridDict[layer].SetValueMultiple(startingCell.x, startingCell.y, sizeOnGrid.x, sizeOnGrid.y, null);
        }

        #region Get items on grid

        public GridItem GetItem(GridLayer layer, int x, int y)
        {
            return _gridDict[layer].GetValue(x, y);
        }

        public GridItem GetItem(GridLayer layer, Vector3 worldPosition)
        {
            return _gridDict[layer].GetValue(worldPosition);
        }

        public List<GridItem> GetAllItems(GridLayer layer)
        {
            var items = new HashSet<GridItem>();
            var grid = _gridDict[GizmoLayer];
            grid.GetGridSize(out var gridWidth, out var gridDepth);
            for (var x = 0; x < gridWidth; x++)
            for (var y = 0; y < gridDepth; y++)
            {
                var item = grid.GetValue(x, y);
                if (item) items.Add(item);
            }

            return items.ToList();
        }

        public List<GridItem> GetItems(GridLayer layer, List<Vector2Int> cells)
        {
            var items = new List<GridItem>();
            foreach (var cell in cells) items.Add(_gridDict[layer].GetValue(cell.x, cell.y));

            return items;
        }

        #endregion

        #region Generic Grid Calculation

        public Vector3 GetOrigin(Vector2Int startingCellPosition, Vector2Int sizeOnGrid)
        {
            return GetCellLeftBottom(startingCellPosition.x, startingCellPosition.y) +
                   new Vector3(sizeOnGrid.x / 2.0f, 0, sizeOnGrid.y / 2.0f) * _cellSize;
        }

        public Vector3 GetOrigin(Vector3 position, Vector2Int sizeOnGrid)
        {
            return GetOriginWithOffset(position, CalculateOffset(sizeOnGrid));
        }

        private Vector3 CalculateOffset(Vector2Int sizeOnGrid)
        {
            return (Vector3.one - new Vector3(sizeOnGrid.x % 2, 1, sizeOnGrid.y % 2)) * halfCellSize;
        }

        private Vector3 GetOriginWithOffset(Vector3 worldPosition, Vector3 offset)
        {
            var gridPosition = GetGridPositionWithOffset(worldPosition, offset);
            return GetOriginWithOffset(gridPosition.x, gridPosition.y, offset);
        }

        private Vector3 GetOriginWithOffset(int x, int z, Vector3 offset)
        {
            return GetLeftBottomWithOffset(x, z, offset) + new Vector3(halfCellSize, 0, halfCellSize);
        }

        private Vector3 GetLeftBottomWithOffset(int x, int z, Vector3 offset)
        {
            return new Vector3(x, 0, z) * _cellSize + _gridLeftBottom + offset;
        }

        private Vector2Int GetGridPositionWithOffset(Vector3 worldPosition, Vector3 offset)
        {
            _defaultGridBase.GetGridPositionWithOffset(worldPosition, offset, out var x, out var z);
            return new Vector2Int(x, z);
        }

        private Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            _defaultGridBase.GetGridPosition(worldPosition, out var x, out var z);
            return new Vector2Int(x, z);
        }

        public Vector3 GetCellLeftBottom(int x, int z)
        {
            return new Vector3(x, 0, z) * _cellSize + _gridLeftBottom;
        }

        public Vector3 GetCellOrigin(int x, int z)
        {
            return GetCellLeftBottom(x, z) + new Vector3(halfCellSize, 0, halfCellSize);
        }

        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            var gridPosition = GetGridPosition(worldPosition);
            return GetCellOrigin(gridPosition.x, gridPosition.y);
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
            if (!_enableGizmo) return;
            if (_camera == null) return;
            if (_gridDict?[GizmoLayer] == null) return;

            Gizmos.color = Color.white;
            for (var x = 0; x < _size.x; x++)
            for (var y = 0; y < _size.y; y++)
            {
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x, y + 1));
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x + 1, y));
            }

            Gizmos.DrawLine(GetCellLeftBottom(0, _size.y), GetCellLeftBottom(_size.x, _size.y));
            Gizmos.DrawLine(GetCellLeftBottom(_size.x, 0), GetCellLeftBottom(_size.x, _size.y));

            var cameraTransform = _camera.transform;
            if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.04f);

            var gizmosColor = Color.gray;
            gizmosColor.a = 0.5f;
            Gizmos.color = gizmosColor;
            var items = GetAllItems(GizmoLayer);

            foreach (var item in items)
                Gizmos.DrawCube(item.transform.position,
                    new Vector3(_cellSize * item.sizeOnGrid.x, 0.1f, _cellSize * item.sizeOnGrid.y));
        }

        #endregion
    }
}