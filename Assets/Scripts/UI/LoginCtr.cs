using Event;
using QFramework;
using UnityEngine;
using Utility;

namespace UI
{
    public class LoginCtr : MonoBehaviour, IController {
        public LoginUI loginUI;
        public PreResLoadUI preResLoadUI;

        private void Start() {
            preResLoadUI.gameObject.SetActiveFast(true);
            loginUI.gameObject.SetActiveFast(false);
            DebugTool.LogWithHexColor("LoginCtr Start");
            this.RegisterEvent<OpenLoginEvent>(OnOpenLogin).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PreResLoadUIFinishEvent>(OnLoadResFinish).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnOpenLogin(OpenLoginEvent e) {
            loginUI.gameObject.SetActiveFast(true);
        }



        private void OnLoadResFinish(PreResLoadUIFinishEvent e) {
            DebugTool.LogWithHexColor("OnLoadResFinish");
            preResLoadUI.gameObject.SetActiveFast(false);
            loginUI.gameObject.SetActiveFast(true);
        }

        public IArchitecture GetArchitecture() {
            return RLSGameArchitecture.Interface;
        }
    }
}
