using System;
using Src.GridSystem;
using UnityEngine;

namespace Src
{
    public class TowerDemoItem : MonoBehaviour, IGridItem
    {
        public void Update()
        {
            
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}