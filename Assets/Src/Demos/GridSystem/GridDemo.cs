using Src.GridSystem;
using UnityEngine;

namespace Src.Demos.GridSystem
{
    public class GridDemo : MonoBehaviour
    {
        private Camera _camera;
        private bool _editMode;
        private GridManager _gridManager;
        private GridItem _selectedItem;

        private void Awake()
        {
            _gridManager = GridManager.instance;
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _editMode = !_editMode;
                Debug.Log("Edit mode: " + _editMode);
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

                if (_selectedItem)
                {
                    if (_selectedItem.PlaceIntoGrid(hit.point)) _selectedItem = null;
                }
                else
                {
                    var item = _gridManager.GetItem(GridLayer.Default, hit.point);
                    if (item)
                    {
                        item.RemoveFromGrid();
                        _selectedItem = item;
                    }
                }
            }

            if (!_selectedItem) return;

            if (Input.GetKeyUp(KeyCode.UpArrow)) _selectedItem.sizeOnGrid += Vector2Int.up;

            if (Input.GetKeyUp(KeyCode.DownArrow)) _selectedItem.sizeOnGrid += Vector2Int.down;

            if (Input.GetKeyUp(KeyCode.LeftArrow)) _selectedItem.sizeOnGrid += Vector2Int.left;

            if (Input.GetKeyUp(KeyCode.RightArrow)) _selectedItem.sizeOnGrid += Vector2Int.right;
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
                foreach (var pos in availability.overlappedItems[item])
                    Gizmos.DrawCube(_gridManager.GetCellOrigin(pos.x, pos.y),
                        new Vector3(_gridManager.cellSize, 0.1f, _gridManager.cellSize));
            }

            mouseWorldPosition.y = 0;
            Transform selectedItemTransform;
            (selectedItemTransform = _selectedItem.transform).position =
                _gridManager.GetOrigin(mouseHit.point, _selectedItem.sizeOnGrid);

            var gizmosColor = Color.green;
            gizmosColor.a = 0.5f;
            Gizmos.color = gizmosColor;
            Gizmos.DrawCube(selectedItemTransform.position,
                new Vector3(_selectedItem.sizeOnGrid.x * _gridManager.cellSize, 0.1f,
                    _selectedItem.sizeOnGrid.y * _gridManager.cellSize));
        }
    }
}