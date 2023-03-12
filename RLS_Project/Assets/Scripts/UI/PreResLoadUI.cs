using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using QFramework;
using RLSGame.AssetManagement;

public class PreResLoadUI : MonoBehaviour, IController
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;
    [SerializeField] string _preLoadLabel = "preload";
    bool isDone;

    private void Start()
    {
        StartLoadPreRes();
    }

    public void StartLoadPreRes()
    {

        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        isDone = false;
        var handle = AssetManager.LoadAssetsByLabelAsync(_preLoadLabel);
        handle.Completed += op =>
        {
            isDone = true;
            progressSlider.value = handle.PercentComplete;
            progressText.text = (int)(progressSlider.value * 100) + "%";
            LoadFinished();
        };

        while (!isDone)
        {
            progressSlider.value = handle.PercentComplete;
            progressText.text = (int)(progressSlider.value * 100) + "%";
            yield return 0f;
        }
        yield return null;
    }

    void LoadFinished()
    {
        this.SendCommand(new PreResLoadUIFinishCommand());
    }

    public IArchitecture GetArchitecture()
    {
        return RLSGameArchitecture.Interface;
    }
}