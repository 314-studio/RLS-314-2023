using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private float cellSize = 0.5f;
        [SerializeField] private Vector2 gridOrigin = new Vector2(0, 0);
        [SerializeField] private Vector2 gridBounds = new Vector2(20, 20);

        
        private Vector2Int _gridSize;
        private Grid<GridItem> _defaultGrid;
        private Dictionary<string, Grid<GridItem>> _gridDict;
        private Vector3 _gridLeftBottom;
        private float _halfCellSize;

        private void Start()
        {
            _gridSize = new Vector2Int(Mathf.FloorToInt(gridBounds.x / cellSize),
                Mathf.FloorToInt(gridBounds.y / cellSize));
            _gridLeftBottom = new Vector3(gridOrigin.x - _gridSize.x * cellSize / 2, 0,
                gridOrigin.y - _gridSize.y * cellSize / 2);
            _halfCellSize = cellSize / 2;
            
            GridItem defaultGridItem = gameObject.AddComponent<GridItem>();
            _gridDict = new Dictionary<string, Grid<GridItem>>();
            _defaultGrid = new Grid<GridItem>(_gridSize.x, _gridSize.y, cellSize, _gridLeftBottom);
            _gridDict.Add(defaultGridItem.Layer, _defaultGrid);
        }

        #region Debug
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (var x = 0; x < _gridSize.x; x++)
            for (var y = 0; y < _gridSize.y; y++)
            {
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x, y + 1));
                Gizmos.DrawLine(GetCellLeftBottom(x, y), GetCellLeftBottom(x + 1, y));
            }

            Gizmos.DrawLine(GetCellLeftBottom(0, _gridSize.y), GetCellLeftBottom(_gridSize.x, _gridSize.y));
            Gizmos.DrawLine(GetCellLeftBottom(_gridSize.x, 0), GetCellLeftBottom(_gridSize.x, _gridSize.y));


            var cameraTransform = Camera.main.transform;
            if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.04f);
            var cellCenter = GetCellOrigin(hit.point);
            Gizmos.color = new Color(1f, 1f, 0, 0.5f);
            Gizmos.DrawCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
        }
        #endregion

        #region Generic Grid Calculation
        private Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            _defaultGrid.GetGridPosition(worldPosition, out var x, out var z);
            return new Vector2Int(x, z);
        }
        
        private Vector3 GetCellLeftBottom(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize + _gridLeftBottom;
        }

        private Vector3 GetCellOrigin(int x, int z)
        {
            return GetCellLeftBottom(x, z) + new Vector3(_halfCellSize, 0, _halfCellSize);
        }

        private Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            Vector2Int gridPosition = GetGridPosition(worldPosition);
            return GetCellOrigin(gridPosition.x, gridPosition.y);
        }
        #endregion

        #region Singleton
        private static GridManager _instance;
        public static GridManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    SetupInstance();
                }

                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<GridManager>();
            if (_instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = "GridManager";
                _instance = gameObj.AddComponent<GridManager>();
            }
        }
        #endregion
    }
}