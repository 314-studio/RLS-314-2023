using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using QFramework;

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
