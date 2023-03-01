using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        private readonly GridManager _gridManager = GridManager.Instance;
        private Vector2Int _startingCellPosition;

        [SerializeField] private Vector2Int _sizeOnGrid = new Vector2Int(1, 1);
        [SerializeField] private GridLayer _layer = GridLayer.Default;

        private Vector2Int _posOnGrid = new Vector2Int(-1, -1);

        public Vector2Int sizeOnGrid
        {
            get => _sizeOnGrid;
            set => _sizeOnGrid = value;
        }

        public GridLayer layer
        {
            get => _layer;
            set => _layer = value;
        }

        private void OnDrawGizmos()
        {
            if (layer != GridManager.GizmoLayer)
            {
                return;
            }

            Color gizmosColor = Color.green;
            gizmosColor.a = 0.5f;
            Gizmos.color = gizmosColor;
            Gizmos.DrawCube(transform.position,
                new Vector3(sizeOnGrid.x * GridManager.CellSize, 0.1f, sizeOnGrid.y * GridManager.CellSize));
        }

        public bool PlaceIntoGrid(Vector3 worldPosition)
        {
            var canPlace = _gridManager.PlaceIntoGrid(this, worldPosition, out var availability);
            _startingCellPosition = availability.startingCellPosition;
            Debug.Log(canPlace);
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

        public void ScaleToGridSize()
        {
            var size = GetComponent<Renderer>().bounds.size;
            var localScale = transform.localScale;
            var scale = new Vector3(sizeOnGrid.x * GridManager.CellSize / size.x * localScale.x,
                localScale.y,
                sizeOnGrid.y * GridManager.CellSize / size.z * localScale.z);
            localScale = scale;
            transform.localScale = localScale;
        }

        protected virtual void Init()
        {
        }
    }
}