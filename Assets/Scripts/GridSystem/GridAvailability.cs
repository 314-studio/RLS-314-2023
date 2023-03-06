using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridAvailability
    {
        public GridAvailability(GridLayer layer, Vector2Int sizeOnGrid, Vector3 origin, Vector2Int startingCellPosition,
            Dictionary<GridItem, List<Vector2Int>> overlappedItems)
        {
            this.layer = layer;
            this.sizeOnGrid = sizeOnGrid;
            this.origin = origin;
            this.startingCellPosition = startingCellPosition;
            this.overlappedItems = overlappedItems;
        }

        public GridLayer layer { get; }
        public Vector2Int sizeOnGrid { get; }
        public Vector3 origin { get; }
        public Vector2Int startingCellPosition { get; }
        public Dictionary<GridItem, List<Vector2Int>> overlappedItems { get; }
    }
}