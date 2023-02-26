using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class GameController : MonoBehaviour,IController
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //GetArchitecture();
    }

    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return RLSGameArchitecture.Interface;
    }

}
