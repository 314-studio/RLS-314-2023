using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 不能重复添加这个组件
[DisallowMultipleComponent]
public class MutiLanguageText : MonoBehaviour, IController
{
    private Text showText;
    public int key;

    void Start()
    {
        showText = GetComponent<Text>();
        this.GetSystem<IConfigSystem>().RegisterText(this);
    }

    public void Reset()
    {
        string s = this.GetSystem<IConfigSystem>().GetText(key);
        if (showText != null && s.Length != 0)
        {
            showText.text = s;
        }
    }

    private void OnEnable()
    {
        if (this.GetSystem<IConfigSystem>() != null)
            Reset();
    }

    private void OnDestroy()
    {
        if (this.GetSystem<IConfigSystem>() != null)
            this.GetSystem<IConfigSystem>().UnregisterText(this);
    }

    public IArchitecture GetArchitecture()
    {
        return RLSGameArchitecture.Interface;
    }
}
