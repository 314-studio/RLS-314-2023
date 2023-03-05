using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        private readonly GridManager _gridManager = GridManager.instance;
        private Vector2Int _startingCellPosition;

        [SerializeField] private Vector2Int _sizeOnGrid = new Vector2Int(1, 1);
        [SerializeField] private GridLayer _layer = GridLayer.Default;

        private Vector2Int _posOnGrid;

        #region properties

        public Vector2Int sizeOnGrid
        {
            get => _sizeOnGrid;
            set => _sizeOnGrid = value;
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