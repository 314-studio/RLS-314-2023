using System;
using System.Collections.Generic;

namespace FrameworkDesign
{
    public class IOCContainer
    {
        Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            Type type = typeof(T);
            if (mInstances.ContainsKey(type))
            {
                mInstances[type] = instance;

            }
            else
            {
                mInstances.Add(type, instance);
            }
        }

        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (mInstances.TryGetValue(type, out var obj))
            {
                return obj as T;
            }
            return null;
        }
    }
}
