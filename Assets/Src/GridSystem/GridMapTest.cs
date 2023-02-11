using System;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridMapTest : MonoBehaviour
    {
        public void Start()
        {
            GridMap map = new GridMap(10, 10, 1.0f, Vector3.zero);

            BuildingDemo building = new BuildingDemo();
            Debug.Log(building.GetType());
            map.SetValue(0, 0, building);
            Debug.Log(map.GetValue<BuildingDemo>(0, 0));
        }
    }
}