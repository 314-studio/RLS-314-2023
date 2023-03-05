using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridAvailability
    {
        public GridLayer layer { get; }
        public Vector2Int sizeOnGrid { get; }
        public Vector3 origin { get; }
        public Vector2Int startingCellPosition { get; }
        public Vector3 startingCellOrigin { get; }
        public Dictionary<GridItem, List<Vector2Int>> overlappedItems { get; }

        private readonly GridManager _gridManager;

        public GridAvailability(GridLayer layer, Vector2Int sizeOnGrid, Vector3 origin, Vector2Int startingCellPosition,
            Dictionary<GridItem, List<Vector2Int>> overlappedItems)
        {
            this.layer = layer;
            this.sizeOnGrid = sizeOnGrid;
            this.origin = origin;
            this.startingCellPosition = startingCellPosition;
            this.overlappedItems = overlappedItems;

            _gridManager = GridManager.instance;
        }
    }
}