using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.GridSystem
{
    public class GridMap
    {
        private readonly int _width;
        private readonly int _depth;
        private readonly float _cellSize;
        private readonly Vector3 _gridOrigin;
        private readonly GameObject _defaultGrid;
        private readonly Dictionary<string, Grid<GameObject>> _gridMap;

        /// <summary>
        /// 网格字典，用来管理游戏中所有的格子
        /// 使用装键值对（key-value pair）实现，key是网tag，value是网格
        /// 任何继承了 @GameObject 的类都可以装进格子里
        /// </summary>
        /// <param name="width">x轴上格子的数量</param>
        /// <param name="depth">z轴上格子的数量</param>
        /// <param name="cellSize">格子大小</param>
        /// <param name="gridOrigin">网格中心位置</param>
        public GridMap(int width, int depth, float cellSize, Vector3 gridOrigin)
        {
            _width = width;
            _depth = depth;
            _cellSize = cellSize;
            _gridOrigin = gridOrigin;
            _defaultGrid = new GameObject();
            _gridMap = new Dictionary<string, Grid<GameObject>>();

            CreateGridIfNull(_defaultGrid.tag);
        }

        /// <summary>
        /// 根据 @GameObject tag获取整个网格 @Grid
        /// </summary>
        /// <param name="tag">存入格子对象tag，默认为类名，E.g. GameObject.tag</param>
        /// <returns>网格</returns>
        public Grid<GameObject> GetGrid(string tag)
        {
            return _gridMap[tag];
        }

        /// <summary>
        /// 根据 @GameObject tag获取网格数组
        /// </summary>
        /// <param name="tag">存入格子对象tag，默认为类名，E.g. GameObject.tag</param>
        /// <returns>底层的网格数组</returns>
        public GameObject[,] GetGridArray(string tag)
        {
            return _gridMap[tag].GetGridArray();
        }

        /// <summary>
        /// 获取网格的大小
        /// </summary>
        /// <param name="width">输出在x轴上格子的数量</param>
        /// <param name="depth">输出在z轴上格子的数量</param>
        public void GetGridSize(out int width, out int depth)
        {
            width = _width;
            depth = _depth;
        }

        /// <summary>
        /// 获取格子的大小
        /// </summary>
        /// <returns>格子边的长度浮点数</returns>
        public float GetCellSize()
        {
            return _cellSize;
        }

        /// <summary>
        /// 设置某个格子的值
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <param name="item">要存入的对象</param>
        public void SetValue(int x, int z, GameObject item)
        {
            CreateGridIfNull(item.tag);
            _gridMap[item.tag].SetValue(x, z, item);
        }

        /// <summary>
        /// 设置某个格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="item">要存入的对象</param>
        public void SetValue(Vector3 worldPosition, GameObject item)
        {
            CreateGridIfNull(item.tag);
            _gridMap[item.tag].SetValue(worldPosition, item);
        }

        /// <summary>
        /// 获取某个格子的值
        /// </summary>
        /// <param name="tag">存入格子的对象的tag</param>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>格子对象</returns>
        public GameObject GetValue(string tag, int x, int z)
        {
            return _gridMap[tag].GetValue(x, z);
        }

        /// <summary>
        /// 获取某个格子的值
        /// </summary>
        /// <param name="tag">存入格子的对象tag</param>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns>格子对象</returns>
        public GameObject GetValue(string tag, Vector3 worldPosition)
        {
            return _gridMap[tag].GetValue(worldPosition);
        }

        /// <summary>
        /// 获取某个格子左下角的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>世界坐标</returns>
        public Vector3 GetCellLeftBottom(int x, int z)
        {
            return _gridMap[_defaultGrid.tag].GetWorldPosition(x, z);
        }

        /// <summary>
        /// 转换世界坐标为格子的位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="x">x轴上格子的位置</param>
        /// <param name="z">z轴上格子的位置</param>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
        {
            _gridMap[_defaultGrid.tag].GetGridPosition(worldPosition, out x, out z);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>格子中心的世界坐标</returns>
        public Vector3 GetCellOrigin(int x, int z)
        {
            return _gridMap[_defaultGrid.tag].GetCellOrigin(x, z);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns>格子中心的世界坐标</returns>
        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            return _gridMap[_defaultGrid.tag].GetCellOrigin(worldPosition);
        }

        /// <summary>
        /// 创建一个网格层
        /// </summary>
        /// <param name="tag">网格对象tag</param>
        private void CreateGridIfNull(string tag)
        {
            if (_gridMap.ContainsKey(tag))
                return;
            _gridMap.Add(tag, new Grid<GameObject>(_width, _depth, _cellSize, _gridOrigin));
        }
    }
}