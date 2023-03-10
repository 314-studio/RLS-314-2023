using UnityEngine;

namespace Asset_Management
{
    public class MonoTracker : MonoBehaviour
    {
        public delegate void DelegateDestroyed(MonoTracker tracker);
        public event DelegateDestroyed OnDestroyed;

        public object key { get; set; }

        void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}