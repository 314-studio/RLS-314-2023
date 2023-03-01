using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using QFramework;

public class UISceneLoadingCtrl : MonoBehaviour, IController
{
    public Slider progressSlider;
    public Text progressText;

    private float timer;
    public float minLoadingTime = 0.1f;

    private void Start()
    {
        this.GetModel<IGameModel>().SceneLoading.Value = true;
        this.GetModel<IGameModel>().SceneLoaded.Value = false;
        progressSlider.value = 0;
        progressText.text = "0%";
        CoroutineController.Instance.StartCoroutine(LoadScene());
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private IEnumerator LoadScene()
    {
        this.GetModel<IGameModel>().SceneLoaded.Value = false;
        progressSlider.value = 0;
        progressText.text = (int)(progressSlider.value * 100) + "%";
        progressSlider.value = 0.1f;
        progressText.text = (int)(progressSlider.value * 100) + "%";
        var async = SceneManager.LoadSceneAsync((int)this.GetModel<IGameModel>().LoadingTargetSceneID.Value);

        while (timer < minLoadingTime)
        {
            var maxProgress = (timer / minLoadingTime);
            progressSlider.value = maxProgress;
            progressText.text = (int)(maxProgress * 100) + "%";
            yield return null;
        }

        while (!async.isDone)
        {
            progressSlider.value = Mathf.Min(async.progress, async.progress);
            progressText.text = (int)(progressSlider.value * 100) + "%";
            yield return null;
        }

        Destroy(gameObject);
        yield return new WaitForSeconds(0.1f);
        this.GetModel<IGameModel>().SceneLoaded.Value = true;
        this.GetModel<IGameModel>().SceneLoading.Value = false;
    }

    public IArchitecture GetArchitecture()
    {
        return RLSGameArchitecture.Interface;
    }
}