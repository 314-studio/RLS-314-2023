using UnityEngine;

namespace Src.GridSystem
{
    public class Grid<T>
    {
        private readonly int _width;
        private readonly int _depth;
        private readonly float _cellSize;
        private readonly Vector3 _gridLeftBottom;
        private readonly T[,] _gridArray;
        
        public Grid(int width, int depth, float cellSize, Vector3 gridLeftBottom)
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
        
        public void SetValue(int x, int z, T value)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth) _gridArray[x, z] = value;
        }
        
        public void SetValue(Vector3 worldPosition, T value)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            SetValue(x, z, value);
        }
        
        public T GetValue(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth)
                return _gridArray[x, z];
            else
                return default;
        }
        
        public T GetValue(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            return GetValue(x, z);
        }
        
        public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - _gridLeftBottom).x / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _gridLeftBottom).z / _cellSize);
        }
    }
}