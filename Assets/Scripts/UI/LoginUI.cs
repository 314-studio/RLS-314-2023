using System;
using Command;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class LoginUI : MonoBehaviour, IController
    {
        public Button loginBtn;

        public IArchitecture GetArchitecture()
        {
            return RLSGameArchitecture.Interface;
        }

        private void Start()
        {
            loginBtn.onClick.AddListener(() => {
                this.GetSystem<ISoundSystem>().PlayClickSound();
                this.SendCommand(new LoadSceneCommand(SceneID.Game));
            });


        }
    }
}
