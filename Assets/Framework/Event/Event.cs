using System;

namespace FrameworkDesign
{
    public class Event<T> where T : Event<T>
    {
        private static Action mOnEvent;

        public static void RegisterEvent(Action OnEvent)
        {
            mOnEvent += OnEvent;
        }

        public static void UnRegisterEvent(Action OnEvent)
        {
            mOnEvent -= OnEvent;
        }

        /// <summary>
        /// 出发事件
        /// </summary>
        public static void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }
}


