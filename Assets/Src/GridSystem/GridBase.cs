using UnityEngine;

namespace Src.GridSystem
{
    public class GridBase<T>
    {
        private readonly float _cellSize;
        private readonly int _depth;
        private readonly T[,] _gridArray;
        private readonly Vector3 _gridLeftBottom;
        private readonly int _width;

        public GridBase(int width, int depth, float cellSize, Vector3 gridLeftBottom)
        {
            _width = width;
            _depth = depth;
            _cellSize = cellSize;
            _gridLeftBottom = gridLeftBottom;
            _gridArray = new T[width, depth];
        }

        public T[,] GetGridArray()
        {
            return _gridArray;
        }

        public void GetGridSize(out int width, out int depth)
        {
            width = _width;
            depth = _depth;
        }

        public void SetValue(int x, int y, T value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _depth) _gridArray[x, y] = value;
        }

        public void SetValue(Vector3 worldPosition, T value)
        {
            GetGridPosition(worldPosition, out var x, out var y);
            SetValue(x, y, value);
        }

        public void SetValueMultiple(int startX, int startY, int expendX, int expendY, T value)
        {
            for (var x = 0; x < expendX; x++)
            for (var y = 0; y < expendY; y++)
                SetValue(x + startX, y + startY, value);
        }

        public T GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _depth)
                return _gridArray[x, y];
            return default;
        }

        public T GetValue(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var y);
            return GetValue(x, y);
        }

        public T[,] GetValueMultiple(int startX, int startY, int expandX, int expandY)
        {
            if (startX < 0 || startY < 0 || expandX < 1 || expandY < 1) return null;
            var tArray = new T[expandX, expandY];
            for (var x = 0; x < expandX; x++)
            for (var y = 0; y < expandY; y++)
                tArray[x, y] = GetValue(x + startX, y + startY);

            return tArray;
        }

        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _gridLeftBottom).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _gridLeftBottom).z / _cellSize);
        }

        public void GetGridPositionWithOffset(Vector3 worldPosition, Vector3 offset, out int x, out int y)
        {
            var pos = worldPosition - _gridLeftBottom - offset;
            x = Mathf.FloorToInt(pos.x / _cellSize);
            y = Mathf.FloorToInt(pos.z / _cellSize);
        }
    }
}