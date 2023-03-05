using UnityEngine;

namespace Src.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        [SerializeField] private Vector2Int _sizeOnGrid = new(1, 1);
        [SerializeField] private GridLayer _layer = GridLayer.Default;
        private readonly GridManager _gridManager = GridManager.instance;

        private Vector2Int _posOnGrid;
        private Vector2Int _startingCellPosition;

        #region properties

        public Vector2Int sizeOnGrid
        {
            get => _sizeOnGrid;
            set
            {
                if (value is { x: > 0, y: > 0 }) _sizeOnGrid = value;
            }
        }

        public Vector2 sizeInWorld => _sizeOnGrid * new Vector2(_gridManager.cellSize, _gridManager.cellSize);

        public GridLayer layer
        {
            get => _layer;
            set => _layer = value;
        }

        #endregion

        #region public methods

        public bool PlaceIntoGrid(Vector3 worldPosition)
        {
            var canPlace = _gridManager.PlaceIntoGrid(this, worldPosition, out var availability);
            _startingCellPosition = availability.startingCellPosition;
            return canPlace;
        }

        public void RemoveFromGrid()
        {
            _gridManager.RemoveFromGrid(layer, _startingCellPosition, sizeOnGrid);
            _startingCellPosition = default;
        }

        public bool GetAvailability(Vector3 worldPosition, out GridAvailability gridAvailability)
        {
            var canPlace = _gridManager.GetAvailability(layer, worldPosition, sizeOnGrid, out var availability);
            gridAvailability = availability;
            return canPlace;
        }

        #endregion
    }
}