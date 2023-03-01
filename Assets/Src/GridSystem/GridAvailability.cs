using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridCell
    {
        public Vector2Int gridPosition { get; }
        public GridItem item { get; }

        public Vector3 origin => GridManager.Instance.GetCellOrigin(gridPosition.x, gridPosition.y);

        public GridCell(Vector2Int gridPosition, GridItem item)
        {
            this.gridPosition = gridPosition;
            this.item = item;
        }
    }


    public class GridAvailability
    {
        public GridLayer layer { get; }
        public Vector2Int sizeOnGrid { get; }
        public Vector3 origin { get; }
        public Vector2Int startingCellPosition { get; }
        public Vector3 startingCellOrigin { get; }
        public List<GridCell> occupiedCells { get; }

        private readonly GridManager _gridManager;

        public GridAvailability(GridLayer layer, Vector2Int sizeOnGrid, Vector3 origin, Vector2Int startingCellPosition,
            Vector3 startingCellOrigin, List<GridCell> occupiedCells)
        {
            this.layer = layer;
            this.sizeOnGrid = sizeOnGrid;
            this.origin = origin;
            this.startingCellPosition = startingCellPosition;
            this.startingCellOrigin = startingCellOrigin;
            this.occupiedCells = occupiedCells;

            _gridManager = GridManager.Instance;
        }
    }
}