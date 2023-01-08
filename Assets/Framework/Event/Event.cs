using System;

namespace FrameworkDesign
{
    public class Event<T> where T : Event<T>
    {
        private static Action mOnEvent;

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="OnEvent"></param>
        public static void RegisterEvent(Action OnEvent)
        {
            mOnEvent += OnEvent;
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="OnEvent"></param>
        public static void UnRegisterEvent(Action OnEvent)
        {
            mOnEvent -= OnEvent;
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        public static void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }
}


