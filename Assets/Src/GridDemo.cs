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

        private GridManager _gridManager;
        private bool _editMode;
        private GameObject _editModeTower;

        public void Start()
        {
            var planeSize = gameObject.GetComponent<MeshRenderer>().bounds.size;
            var planeScale = new Vector3(gridWidth * gridCellSize / planeSize.x, 0.02f,
                gridDepth * gridCellSize / planeSize.z);
            gameObject.transform.localScale = planeScale;

            _gridManager = gameObject.AddComponent<GridManager>();
            _editModeTower = Instantiate(turretTower);
        }
    }
}