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
        private readonly IGridItem _defaultGrid;
        private readonly Dictionary<string, Grid<IGridItem>> _gridMap;

        /// <summary>
        /// 网格字典，用来管理游戏中所有的格子
        /// 使用装键值对（key-value pair）实现，key是网格类型，value是网格
        /// 任何继承了 @IGridItem 的类都可以装进格子里
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
            _defaultGrid = new DefaultGrid();
            _gridMap = new Dictionary<string, Grid<IGridItem>>();
            
            CreateGridIfNull(_defaultGrid.Type);
        }

        /// <summary>
        /// 根据 @IGridItem 的类型获取整个网格 @Grid
        /// </summary>
        /// <param name="type">存入格子对象的类型，默认为类名，E.g. IGridItem.Type</param>
        /// <returns>网格</returns>
        public Grid<IGridItem> GetGrid(string type)
        {
            return _gridMap[type];
        }

        /// <summary>
        /// 根据 @IGridItem 的类型获取网格数组
        /// </summary>
        /// <param name="type">存入格子对象的类型，默认为类名，E.g. IGridItem.Type</param>
        /// <returns>底层的网格数组</returns>
        public IGridItem[,] GetGridArray(string type)
        {
            return _gridMap[type].GetGridArray();
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
        public void SetValue(int x, int z, IGridItem item)
        {
            CreateGridIfNull(item.Type);
            _gridMap[item.Type].SetValue(x, z, item);
        }

        /// <summary>
        /// 设置某个格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="item">要存入的对象</param>
        public void SetValue(Vector3 worldPosition, IGridItem item)
        {
            CreateGridIfNull(item.Type);
            _gridMap[item.Type].SetValue(worldPosition, item);
        }

        /// <summary>
        /// 获取某个格子的值
        /// </summary>
        /// <param name="type">存入格子的对象的类型</param>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>格子对象</returns>
        public IGridItem GetValue(string type, int x, int z)
        {
            return _gridMap[type].GetValue(x, z);
        }

        /// <summary>
        /// 获取某个格子的值
        /// </summary>
        /// <param name="type">存入格子的对象的类型</param>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns>格子对象</returns>
        public IGridItem GetValue(string type, Vector3 worldPosition)
        {
            return _gridMap[type].GetValue(worldPosition);
        }

        /// <summary>
        /// 获取某个格子左下角的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>世界坐标</returns>
        public Vector3 GetCellLeftBottom(int x, int z)
        {
            return _gridMap[_defaultGrid.Type].GetWorldPosition(x, z);
        }

        /// <summary>
        /// 转换世界坐标为格子的位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="x">x轴上格子的位置</param>
        /// <param name="z">z轴上格子的位置</param>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
        {
            _gridMap[_defaultGrid.Type].GetGridPosition(worldPosition, out x, out z);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns>格子中心的世界坐标</returns>
        public Vector3 GetCellOrigin(int x, int z)
        {
            return _gridMap[_defaultGrid.Type].GetCellOrigin(x, z);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns>格子中心的世界坐标</returns>
        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            return _gridMap[_defaultGrid.Type].GetCellOrigin(worldPosition);
        }
        
        /// <summary>
        /// 创建一个网格层
        /// </summary>
        /// <param name="type">网格对象的类型</param>
        private void CreateGridIfNull(string type)
        {
            if (_gridMap.ContainsKey(type))
                return;
            _gridMap.Add(type, new Grid<IGridItem>(_width, _depth, _cellSize, _gridOrigin));
        }
    }

    internal class DefaultGrid : IGridItem { }
}