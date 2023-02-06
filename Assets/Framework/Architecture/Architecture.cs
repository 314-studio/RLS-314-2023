using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public interface IArchitecture
    {
        /// <summary>
        /// 注册System
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 获取工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 获取model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;
        /// <summary>
        /// 获取系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;

        void SendCommand<T>() where T : ICommand, new();

        void SendCommand<T>(T command) where T : ICommand;

        void SendEvent<T>() where T : new();

        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        void UnRegisterEvent<T>(Action<T> onEvent);

    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture;

        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        private bool mInited = false;

        private List<IModel> mModels = new List<IModel>();
        private List<ISystem> mSystems = new List<ISystem>();

        public static Action<T> OnRegisterPatch = temp => { };

        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                };

                return mArchitecture;
            }
        }

        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                //模块初始化
                mArchitecture.Init();
                OnRegisterPatch?.Invoke(mArchitecture);

                //模块内的model初始化
                foreach (var item in mArchitecture.mModels)
                {
                    item.Init();
                }
                mArchitecture.mModels.Clear();

                //模块内的system初始化
                foreach (var item in mArchitecture.mSystems)
                {
                    item.Init();
                }
                mArchitecture.mSystems.Clear();

                mArchitecture.mInited = true;
            }
        }

        protected abstract void Init();

        private IOCContainer mContainer = new IOCContainer();
        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();
            return mArchitecture.mContainer.Get<T>();
        }

        public static void Register<T>(T instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register<T>(instance);
        }

        /// <summary>
        /// 注册model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void RegisterModel<T>(T model) where T : IModel
        {
            //model赋值
            model.SetArchitecture(this);
            mContainer.Register<T>(model);

            if (!mInited)
            {
                mModels.Add(model);
            }
            else
            {
                model.Init();
            }
        }


        public T GetModel<T>() where T : class, IModel
        {
            return mContainer.Get<T>();
        }

        public void RegisterUtility<T>(T instance) where T : IUtility
        {
            mContainer.Register<T>(instance);
        }

        public T GetUtility<T>() where T : class, IUtility
        {
            return mContainer.Get<T>();
        }



        public void RegisterSystem<T>(T system) where T : ISystem
        {
            //System赋值
            system.SetArchitecture(this);
            mContainer.Register<T>(system);

            if (!mInited)
            {
                mSystems.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        public T1 GetSystem<T1>() where T1 :class, ISystem
        {
            return mContainer.Get<T1>();
        }

        public void SendCommand<T1>() where T1 : ICommand, new()
        {
            var command = new T1();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<T1>(T1 command) where T1 : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<T1>() where T1 : new()
        {
            mTypeEventSystem.Send<T1>();
        }

        public void SendEvent<T1>(T1 e)
        {
            mTypeEventSystem.Send<T1>(e);
        }

        public IUnRegister RegisterEvent<T1>(Action<T1> onEvent)
        {
            return mTypeEventSystem.Register<T1>(onEvent);
        }

        public void UnRegisterEvent<T1>(Action<T1> onEvent)
        {
            mTypeEventSystem.UnRegister<T1>(onEvent);
        }
    }
}

