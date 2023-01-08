using System;

namespace FrameworkDesign
{
    public class Event<T> where T : Event<T>
    {
        private static Action mOnEvent;

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="OnEvent"></param>
        public static void RegisterEvent(Action OnEvent)
        {
            mOnEvent += OnEvent;
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="OnEvent"></param>
        public static void UnRegisterEvent(Action OnEvent)
        {
            mOnEvent -= OnEvent;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        public static void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }
}


