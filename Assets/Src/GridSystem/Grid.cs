using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Src.GridSystem
{
    public class Grid<TGridObject> : IGrid<TGridObject>
    {
        private int _width;
        private int _depth;
        private float _cellSize;
        private float _halfCellSize;
        private Vector3 _leftBottom;

        private TGridObject[,] _gridArray;
        

        public Grid(int width, int depth, float cellSize, Vector3 originPosition)
        {
            Vector3 leftBottom = new Vector3(originPosition.x - width * cellSize / 2, 0,
                originPosition.z - depth * cellSize / 2);
            _width = width;
            _depth = depth;
            _cellSize = cellSize;
            _halfCellSize = _cellSize / 2;
            _leftBottom = leftBottom;

            _gridArray = new TGridObject[width, depth];
        }

        public void GetGridSize(out int width, out int depth)
        {
            width = _width;
            depth = _depth;
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public virtual void SetValue(int x, int z, TGridObject value)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth)
            {
                _gridArray[x, z] = value;
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject value)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            SetValue(x, z, value);
        }

        public TGridObject GetValue(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth)
            {
                return _gridArray[x, z];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            return GetValue(x, z);
        }
        
        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * _cellSize + _leftBottom;
        }

        public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - _leftBottom).x / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _leftBottom).z / _cellSize);
        }

        public Vector3 GetCellOrigin(int x, int z)
        {
            return GetWorldPosition(x, z) + new Vector3(_halfCellSize, 0, _halfCellSize);
        }

        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            return GetCellOrigin(x, z);
        }
    }
}