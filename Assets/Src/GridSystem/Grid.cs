using UnityEngine;

namespace Src.GridSystem
{
    public class Grid<TGridItem>
    {
        private readonly int _width;
        private readonly int _depth;
        private readonly float _cellSize;
        private readonly float _halfCellSize;
        private readonly Vector3 _leftBottom;

        private readonly TGridItem[,] _gridArray;


        /// <summary>
        /// 创建一个网格
        /// </summary>
        /// <param name="width">网格的宽度，x轴上的格子数量</param>
        /// <param name="depth">网格的深度，z轴上的格子数量</param>
        /// <param name="cellSize">网格的大小</param>
        /// <param name="originPosition">网格中心的位置</param>
        public Grid(int width, int depth, float cellSize, Vector3 originPosition)
        {
            var leftBottom = new Vector3(originPosition.x - width * cellSize / 2, 0,
                originPosition.z - depth * cellSize / 2);
            _width = width;
            _depth = depth;
            _cellSize = cellSize;
            _halfCellSize = _cellSize / 2;
            _leftBottom = leftBottom;

            _gridArray = new TGridItem[width, depth];
        }

        /// <summary>
        /// out返回x和z轴上格子的数量
        /// </summary>
        /// <param name="width">x轴上格子的数量</param>
        /// <param name="depth">z轴上格子的数量</param>
        public void GetGridSize(out int width, out int depth)
        {
            width = _width;
            depth = _depth;
        }

        /// <summary>
        /// 返回世界尺寸正方形格子的宽度
        /// </summary>
        /// <returns>世界尺寸格子的宽度</returns>
        public float GetCellSize()
        {
            return _cellSize;
        }

        /// <summary>
        /// 获取网格数组
        /// </summary>
        /// <returns>网格数组</returns>
        public TGridItem[,] GetGridArray()
        {
            return _gridArray;
        }

        /// <summary>
        /// 根据格子数组的位置设置格子的值
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <param name="value">在网格数组此处设置的值</param>
        public void SetValue(int x, int z, TGridItem value)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth) _gridArray[x, z] = value;
        }

        /// <summary>
        /// 根据世界坐标的位置设置格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="value">值</param>
        public void SetValue(Vector3 worldPosition, TGridItem value)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            SetValue(x, z, value);
        }

        /// <summary>
        /// 根据格子数组的位置获取格子的值
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns></returns>
        public TGridItem GetValue(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _depth)
                return _gridArray[x, z];
            else
                return default;
        }

        /// <summary>
        /// 根据世界坐标获取格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns></returns>
        public TGridItem GetValue(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            return GetValue(x, z);
        }

        /// <summary>
        /// 获取格子数组位置的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * _cellSize + _leftBottom;
        }

        /// <summary>
        /// 获取世界坐标的格子位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="x">返回的格子在x轴上的位置</param>
        /// <param name="z">返回的格子在z轴上的位置</param>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - _leftBottom).x / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _leftBottom).z / _cellSize);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns></returns>
        public Vector3 GetCellOrigin(int x, int z)
        {
            return GetWorldPosition(x, z) + new Vector3(_halfCellSize, 0, _halfCellSize);
        }

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns></returns>
        public Vector3 GetCellOrigin(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var z);
            return GetCellOrigin(x, z);
        }
    }
}