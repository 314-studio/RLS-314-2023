using System;
using Src.GridSystem;
using UnityEngine;

namespace Src
{
    public class GridDemo : MonoBehaviour
    {
        [Space] [SerializeField] private int gridWidth = 20;

        [SerializeField] private int gridDepth = 20;

        [SerializeField] private float gridCellSize = 0.2f;

        [Space] [SerializeField]
        private GameObject turrentTower;

        private GridMap _gridMap;
        private Boolean _editMode;
        private GameObject _editModeTower;

        public void Start()
        {
            Vector3 planeSize = gameObject.GetComponent<MeshRenderer>().bounds.size;
            Vector3 planeScale = new Vector3(gridWidth * gridCellSize / planeSize.x, 0.02f,
                gridDepth * gridCellSize / planeSize.z);
            gameObject.transform.localScale = planeScale;

            _gridMap = new GridMap(gridWidth, gridDepth, gridCellSize, Vector3.zero);
            _editModeTower = Instantiate(turrentTower);
            Debug.Log(_editModeTower.GetType());
        }

        public void Update()
        {
            if (Input.GetKeyUp("space"))
            {
                _editMode = !_editMode;
            }

            if (_editMode)
            {
                var cameraTransform = Camera.main.transform;
                if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                    return;

                Vector3 currentGridCellCenter = _gridMap.GetCellOrigin(hit.point);
                _editModeTower.transform.position = currentGridCellCenter;
                
                if (Input.GetKeyUp(KeyCode.P))
                {
                    // _gridMap.SetValue(hit.point, );
                }
            }
            
            
        }

        private void OnDrawGizmos()
        {
            if (_gridMap == null) return;

            _gridMap.GetGridSize(out var width, out var depth);
            Gizmos.color = Color.white;
            for (var x = 0; x < width; x++)
            for (var z = 0; z < depth; z++)
            {
                Gizmos.DrawLine(_gridMap.GetCellLeftBottom(x, z), _gridMap.GetCellLeftBottom(x, z + 1));
                Gizmos.DrawLine(_gridMap.GetCellLeftBottom(x, z), _gridMap.GetCellLeftBottom(x + 1, z));
            }

            Gizmos.DrawLine(_gridMap.GetCellLeftBottom(0, depth), _gridMap.GetCellLeftBottom(width, depth));
            Gizmos.DrawLine(_gridMap.GetCellLeftBottom(width, 0), _gridMap.GetCellLeftBottom(width, depth));


            var cameraTransform = Camera.main.transform;
            if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.04f);
            var cellCenter = _gridMap.GetCellOrigin(hit.point);
            Gizmos.color = new Color(1f, 1f, 0, 0.5f);
            Gizmos.DrawCube(cellCenter, new Vector3(_gridMap.GetCellSize(), 0.1f, _gridMap.GetCellSize()));
        }
    }
}