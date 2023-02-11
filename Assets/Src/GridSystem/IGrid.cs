

using UnityEngine;

namespace Src.GridSystem
{
    public interface IGrid<TGridObject>
    {
        /// <summary>
        /// out返回x和z轴上格子的数量
        /// </summary>
        /// <param name="width">x轴上格子的数量</param>
        /// <param name="depth">z轴上格子的数量</param>
        void GetGridSize(out int width, out int depth);
        
        /// <summary>
        /// 返回世界尺寸正方形格子的宽度
        /// </summary>
        /// <returns>世界尺寸格子的宽度</returns>
        float GetCellSize();

        /// <summary>
        /// 根据格子数组的位置设置格子的值
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <param name="value">在网格数组此处设置的值</param>
        void SetValue(int x, int z, TGridObject value);

        /// <summary>
        /// 根据世界坐标的位置设置格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="value">值</param>
        void SetValue(Vector3 worldPosition, TGridObject value);

        /// <summary>
        /// 根据格子数组的位置获取格子的值
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns></returns>
        TGridObject GetValue(int x, int z);

        /// <summary>
        /// 根据世界坐标获取格子的值
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns></returns>
        TGridObject GetValue(Vector3 worldPosition);

        /// <summary>
        /// 获取格子数组位置的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns></returns>
        Vector3 GetWorldPosition(int x, int z);

        /// <summary>
        /// 获取世界坐标的格子位置
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="x">返回的格子在x轴上的位置</param>
        /// <param name="z">返回的格子在z轴上的位置</param>
        void GetGridPosition(Vector3 worldPosition, out int x, out int z);

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="x">x轴上的第几个格子</param>
        /// <param name="z">z轴上的第几个格子</param>
        /// <returns></returns>
        Vector3 GetCellOrigin(int x, int z);

        /// <summary>
        /// 获取格子中心的世界坐标
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns></returns>
        Vector3 GetCellOrigin(Vector3 worldPosition);
    }
}