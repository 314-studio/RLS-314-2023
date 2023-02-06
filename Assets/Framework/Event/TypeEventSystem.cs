using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);

        IUnRegister Register<T>(Action<T> onEvene);
        void UnRegister<T>(Action<T> onEvene);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }

    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem typeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            typeEventSystem.UnRegister<T>(OnEvent);

            typeEventSystem = null;

            OnEvent = null;
        }
    }

    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var item in mUnRegisters)
            {
                item.UnRegister();
            }
            mUnRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }
            trigger.AddUnRegister(unRegister);
        }
    }

    public class TypeEventSystem : ITypeEventSystem
    {

        public interface IRegistrations
        {

        }

        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();


        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {

            }
            else
            {
                registrations = new Registrations<T>();
                mEventRegistration.Add(type, registrations);
            }
            (registrations as Registrations<T>).OnEvent += onEvent;
            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                typeEventSystem = this
            };
        }

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}

