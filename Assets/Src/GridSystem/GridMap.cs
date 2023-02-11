using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridMap
    {
        private Grid<Dictionary<string, GridItem>> _grid;
        public GridMap(int width, int depth, float cellSize, Vector3 originPosition)
        {
            _grid = new Grid<Dictionary<string, GridItem>>(width, depth, cellSize, originPosition);
        }

        public void GetGridSize(out int width, out int depth)
        {
            _grid.GetGridSize(out width, out depth);
        }

        public float GetCellSize()
        {
            return _grid.GetCellSize();
        }

        public void SetValue(int x, int z, GridItem value)
        {
            _grid.SetValue(x, z, new Dictionary<string, GridItem>());
            var objectDict = _grid.GetValue(x, z);
            objectDict.Add(value.GetClassName(), value);
        }

        public GridItem GetValue<IGridItem>(int x, int z)
        {
            return _grid.GetValue(x, z)[nameof(IGridItem)];
        }
    }
}