using Src.GridSystem;
using UnityEngine;

namespace Src
{
    public class GridDemo : MonoBehaviour
    {
        [Space] [SerializeField] private GameObject _cube;

        private GridManager _gridManager;
        private bool _editMode;
        private Camera _camera;
        private GridItem _selectedItem;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Start()
        {
            _gridManager = gameObject.AddComponent<GridManager>();
        }

        private void OnDrawGizmos()
        {
            if (!_selectedItem) return;
            if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var mouseHit, 100.0f))
                return;
            var mouseWorldPosition = mouseHit.point;
            if (!_selectedItem.GetAvailability(mouseWorldPosition, out var availability))
            {
                Gizmos.color = Color.red;
                var items = availability.overlappedItems;
                foreach (var item in items.Keys)
                {
                    foreach (var pos in availability.overlappedItems[item])
                    {
                        Gizmos.DrawCube(_gridManager.GetCellOrigin(pos.x, pos.y), new Vector3(GridManager.CellSize, 0.1f, GridManager.CellSize));
                    }
                }
            }

            mouseWorldPosition.y = 0;
            _selectedItem.transform.position = _gridManager.GetOrigin(mouseHit.point, _selectedItem.sizeOnGrid);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.K))
            {
                var a = 1;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _editMode = !_editMode;
                Debug.Log("Edit mode: " + _editMode.ToString());
            }

            if (!_editMode) return;

            if (Input.GetKeyUp(KeyCode.C))
            {
                if (_selectedItem) Destroy(_selectedItem.gameObject);
                var gObject = new GameObject();
                _selectedItem = gObject.AddComponent<GridItem>();
            }

            if (Input.GetMouseButtonUp(0))
            {
                var cameraTransform = _camera.transform;
                if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                    return;

                GridItem item = _gridManager.GetItem(GridLayer.Default, hit.point);
                if (item)
                {
                    _selectedItem = _selectedItem == item ? null : item;
                }

                if (_selectedItem)
                {
                    if (_selectedItem.PlaceIntoGrid(hit.point))
                    {
                        _selectedItem = null;
                    }
                }
            }

            var scaleChanged = false;
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                _selectedItem.sizeOnGrid += Vector2Int.up;
                scaleChanged = true;
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                _selectedItem.sizeOnGrid += Vector2Int.down;
                scaleChanged = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _selectedItem.sizeOnGrid += Vector2Int.left;
                scaleChanged = true;
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                _selectedItem.sizeOnGrid += Vector2Int.right;
                scaleChanged = true;
            }

            // if (scaleChanged) _selectedItem.ScaleToGridSize();
        }
    }
}