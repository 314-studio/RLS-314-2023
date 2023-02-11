using System;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridDemo : MonoBehaviour
    {
        private Grid<bool> _grid;
        private MeshRenderer _renderer;
        private Vector3 _planeSize;

        private void Start()
        {
            _renderer = gameObject.GetComponent<MeshRenderer>();
            _planeSize = _renderer.bounds.size;
            var cellSize = 0.4f;
            var gridWidth = Mathf.CeilToInt(_planeSize.x / cellSize);
            var gridHeight = Mathf.CeilToInt(_planeSize.z / cellSize);
            _grid = new Grid<bool>(gridWidth, gridHeight, cellSize, Vector3.zero);
        }

        private void OnDrawGizmos()
        {
            if (_grid == null) return;

            _grid.GetGridSize(out var width, out var depth);
            Gizmos.color = Color.white;
            for (var x = 0; x < width; x++)
            for (var z = 0; z < depth; z++)
            {
                Gizmos.DrawLine(_grid.GetWorldPosition(x, z), _grid.GetWorldPosition(x, z + 1));
                Gizmos.DrawLine(_grid.GetWorldPosition(x, z), _grid.GetWorldPosition(x + 1, z));
            }

            Gizmos.DrawLine(_grid.GetWorldPosition(0, depth), _grid.GetWorldPosition(width, depth));
            Gizmos.DrawLine(_grid.GetWorldPosition(width, 0), _grid.GetWorldPosition(width, depth));


            var cameraTransform = Camera.main.transform;
            if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.04f);
            var cellCenter = _grid.GetCellOrigin(hit.point);
            Gizmos.color = new Color(1f, 1f, 0, 0.5f);
            Gizmos.DrawCube(cellCenter, new Vector3(_grid.GetCellSize(), 0.1f, _grid.GetCellSize()));
        }
    }
}