using QFramework;
using UI;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour, IController
    {
        public LoginUI loginUI;
        public PreResLoadUI preResLoadUI;
        public IArchitecture GetArchitecture()
        {
            return RLSGameArchitecture.Interface;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            //GetArchitecture();
        }

        void Start()
        {
            //preResLoadUI.StartLoadPreRes();
            //this.GetSystem<ConfigSystem>();
        }

    }
}
