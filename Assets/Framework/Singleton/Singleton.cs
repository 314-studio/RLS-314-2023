﻿using System;
using System.Reflection;

namespace FrameworkDesign
{
    public class Singleton<T> where T :Singleton<T>
    {

        private static T mInstance;

        public static T Instance
        {
            get
            {
                if(mInstance==null)
                {
                    var type = typeof(T);
                    var constructInfo = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    var ctor = Array.Find(constructInfo, c => c.GetParameters().Length == 0);

                    if(ctor == null)
                    {
                        throw new Exception("Non Public Constructor Not Found in" + type.Name);
                    }

                    mInstance = ctor.Invoke(null) as T;
                }
                return mInstance;
            }
        }
    }
}

