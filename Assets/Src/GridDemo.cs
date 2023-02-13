using System;
using Src.GridSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Src
{
    public class GridDemo : MonoBehaviour
    {
        [Space] [SerializeField] private int gridWidth = 20;

        [SerializeField] private int gridDepth = 20;

        [SerializeField] private float gridCellSize = 0.2f;

        [Space] [SerializeField] private GameObject turretTower;

        private GridMap _gridMap;
        private bool _editMode;
        private GameObject _editModeTower;

        public void Start()
        {
            var planeSize = gameObject.GetComponent<MeshRenderer>().bounds.size;
            var planeScale = new Vector3(gridWidth * gridCellSize / planeSize.x, 0.02f,
                gridDepth * gridCellSize / planeSize.z);
            gameObject.transform.localScale = planeScale;

            _gridMap = new GridMap(gridWidth, gridDepth, gridCellSize, Vector3.zero);
            _editModeTower = Instantiate(turretTower);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space)) _editMode = !_editMode;

            if (_editMode)
            {
                var cameraTransform = Camera.main.transform;
                if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 100.0f))
                    return;

                _gridMap.GetGridPosition(hit.point, out var x, out var z);

                var currentGridCellCenter = _gridMap.GetCellOrigin(x, z);
                _editModeTower.transform.position = currentGridCellCenter;

                if (Input.GetKeyUp(KeyCode.P))
                {
                    var obj = _gridMap.GetValue(_editModeTower.tag, x, z);
                    if (!obj)
                    {
                        var newTower = Instantiate(turretTower, currentGridCellCenter, new Quaternion());
                        Debug.Log("放置新的对象，tag为：" + newTower.tag);
                        _gridMap.SetValue(x, z, newTower);
                    }
                }

                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    var obj = _gridMap.GetValue(_editModeTower.tag, x, z);
                    if (obj)
                    {
                        Destroy(obj);
                        Debug.Log("删除了对象，tag为：" + obj.tag);
                    }
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