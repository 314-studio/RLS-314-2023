using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Src.GridSystem
{
    public enum GridLayer
    {
        Default
    }

    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2 _gridBounds = new(20, 20);

        public static GridLayer GizmoLayer = GridLayer.Default;

        private readonly Vector2 _gridOrigin = new(0, 0);

        private Vector2Int _gridSize;
        private Grid<GridItem> _defaultGrid;
        private Dictionary<GridLayer, Grid<GridItem>> _gridDict;
        private Vector3 _gridLeftBottom;

        public Vector2 gridBounds
        {
            get => _gridBounds;
            set => _gridBounds = value;
        }

        public const float CellSize = 0.5f;
        public const float HalfCellSize = 0.25f;

        private void Start()
        {
            _gridSize = new Vector2Int(Mathf.FloorToInt(_gridBounds.x / CellSize),
                Mathf.FloorToInt(_gridBounds.y / CellSize));
            _gridLeftBottom = new Vector3(_gridOrigin.x - _gridSize.x * CellSize / 2, 0,
                _gridOrigin.y - _gridSize.y * CellSize / 2);

            _gridDict = new Dictionary<GridLayer, Grid<GridItem>>();
            _defaultGrid = new Grid<GridItem>(_gridSize.x, _gridSize.y, CellSize, _gridLeftBottom);
            _gridDict.Add(GridLayer.Default, _defaultGrid);
        }

        #region Debug

        private void OnDrawGizmos()
        {
            if (Camera.main == null) return;

            Gizmos.color = Color.white;
            for (var x = 0; x < _gridSize.x; x++)
            for (var y = 0; y < _gridSize.y; y++)
            {
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x, y + 1));
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x + 1, y));
            }

            Gizmos.DrawLine(GetCellLeftBottom(0, _gridSize.y), GetCellLeftBottom(_gridSize.x, _gridSize.y));
            Gizmos.DrawLine(GetCellLeftBottom(_gridSize.x, 0), GetCellLeftBottom(_gridSize.x, _gridSize.y));

            var cameraTransform = Camera.main.transform;
            if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.04f);

            Color gizmosColor = Color.gray;
            gizmosColor.a = 0.5f;
            Gizmos.color = gizmosColor;
            List<GridItem> items = GetAllItems(GizmoLayer);
            if (items.Count > 0)
            {
                // Debug.Log("not empty " + items.Count);
            }

            foreach (var item in items)
            {
                Gizmos.DrawCube(item.transform.position,
                    new Vector3(CellSize * item.sizeOnGrid.x, 0.1f, CellSize * item.sizeOnGrid.y));
            }
        }

        #endregion


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
            var origin = GetOriginWithOffset(position, offset);

            var overlappedItems = new Dictionary<GridItem, List<Vector2Int>>();
            var itemArray = _gridDict[layer]
                .GetValueMultiple(startingGridPosition.x, startingGridPosition.y, sizeOnGrid.x, sizeOnGrid.y);
            for (var x = 0; x < sizeOnGrid.x; x++)
            {
                for (var y = 0; y < sizeOnGrid.y; y++)
                {
                    var item = itemArray[x, y];
                    if (item)
                    {
                        if (!overlappedItems.ContainsKey(item))
                        {
                            overlappedItems[item] = new List<Vector2Int>();
                        }

                        overlappedItems[item]
                            .Add(new Vector2Int(startingGridPosition.x + x, startingGridPosition.y + y));
                        canPlace = false;
                    }
                }
            }

            availability = new GridAvailability(layer, sizeOnGrid, origin, startingGridPosition,
                overlappedItems);

            return canPlace;
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
            HashSet<GridItem> items = new HashSet<GridItem>();
            Grid<GridItem> grid = _gridDict[GizmoLayer];
            grid.GetGridSize(out var gridWidth, out var gridDepth);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridDepth; y++)
                {
                    GridItem item = grid.GetValue(x, y);
                    if (item)
                    {
                        items.Add(item);
                    }
                }
            }

            return items.ToList();
        }

        public List<GridItem> GetItems(GridLayer layer, List<Vector2Int> cells)
        {
            List<GridItem> items = new List<GridItem>();
            foreach (var cell in cells)
            {
                items.Add(_gridDict[layer].GetValue(cell.x, cell.y));
            }

            return items;
        }

        #endregion

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

        #region Generic Grid Calculation

        public Vector3 GetOrigin(Vector3 position, Vector2Int sizeOnGrid)
        {
            return GetOriginWithOffset(position, CalculateOffset(sizeOnGrid));
        }

        private Vector3 CalculateOffset(Vector2Int sizeOnGrid)
        {
            return (Vector3.one - new Vector3(sizeOnGrid.x % 2, 1, sizeOnGrid.y % 2)) * HalfCellSize;
        }

        private Vector3 GetOriginWithOffset(Vector3 worldPosition, Vector3 offset)
        {
            var gridPosition = GetGridPositionWithOffset(worldPosition, offset);
            return GetOriginWithOffset(gridPosition.x, gridPosition.y, offset);
        }

        private Vector3 GetOriginWithOffset(int x, int z, Vector3 offset)
        {
            return GetLeftBottomWithOffset(x, z, offset) + new Vector3(HalfCellSize, 0, HalfCellSize);
        }

        private Vector3 GetLeftBottomWithOffset(int x, int z, Vector3 offset)
        {
            return new Vector3(x, 0, z) * CellSize + _gridLeftBottom + offset;
        }

        private Vector2Int GetGridPositionWithOffset(Vector3 worldPosition, Vector3 offset)
        {
            _defaultGrid.GetGridPositionWithOffset(worldPosition, offset, out var x, out var z);
            return new Vector2Int(x, z);
        }

        private Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            _defaultGrid.GetGridPosition(worldPosition, out var x, out var z);
            return new Vector2Int(x, z);
        }

        public Vector3 GetCellLeftBottom(int x, int z)
        {
            return new Vector3(x, 0, z) * CellSize + _gridLeftBottom;
        }

        public Vector3 GetCellOrigin(int x, int z)
        {
            return GetCellLeftBottom(x, z) + new Vector3(HalfCellSize, 0, HalfCellSize);
        }

        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            var gridPosition = GetGridPosition(worldPosition);
            return GetCellOrigin(gridPosition.x, gridPosition.y);
        }

        #endregion

        #region Singleton

        private static GridManager _instance;

        public static GridManager Instance
        {
            get
            {
                if (_instance == null) SetupInstance();

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<GridManager>();
            if (_instance == null)
            {
                var gameObj = new GameObject();
                gameObj.name = "GridManager";
                _instance = gameObj.AddComponent<GridManager>();
            }
        }

        #endregion
    }
}