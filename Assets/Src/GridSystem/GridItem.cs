using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        private GridManager _gridManager;
        private Transform _transform;
        
        [SerializeField] private Vector2Int sizeOnGrid = new Vector2Int(1, 1);
        [SerializeField] private string layer;

        public Vector2Int SizeOnGrid
        {
            get => sizeOnGrid;
            set => sizeOnGrid = value;
        }

        public string Layer
        {
            get => layer ?? GetType().Name;
            set => layer = value;
        }

        private void Start()
        {
            _gridManager = GridManager.Instance;
            _transform = gameObject.GetComponent<Transform>();
            
            Init();
        }
        
        protected virtual void Init() {}
    }
}