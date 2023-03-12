using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

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
