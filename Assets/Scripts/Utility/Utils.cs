using UnityEngine;

namespace Utility
{
    public static class GameObjectExtension
    {
        public static void SetActiveFast(this GameObject o, bool s)
        {
            if (o.activeSelf != s)
            {
                o.SetActive(s);
            }
        }
    }
    public enum SceneID
    {
        Login = 0,
        Loading = 1,
        Game = 2
    }

    public static class Util
    {

    }
}

