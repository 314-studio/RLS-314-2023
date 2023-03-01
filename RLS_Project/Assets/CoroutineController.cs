using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController : MonoBehaviour
{
    public static CoroutineController Instance = null;

    private void Awake()
    {
        DebugTool.LogWithHexColor("CoroutineController Awake");
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
  
    public static Coroutine DelayedCall(Action callback, float delaySec)
           => Instance.StartCoroutine(Instance.CallCoroutine(callback, delaySec));

    public static Coroutine WaitCallUntil(Func<bool> predicate, Action callback)
        => Instance.StartCoroutine(Instance.CallUntilCoroutine(predicate, callback));

    public static void CancelCall(Coroutine cor)
    {
        if (cor != null) Instance.StopCoroutine(cor);
    }

    private IEnumerator CallCoroutine(Action callback, float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        callback.Invoke();
    }

    private IEnumerator CallUntilCoroutine(Func<bool> predicate, Action callback)
    {
        yield return new WaitUntil(predicate);
        callback.Invoke();
    }

    public static void WaitNextFrameCallback(Action callback)
    {
        Instance.StartCoroutine(NextFrameCallback(callback));
    }
    private static IEnumerator NextFrameCallback(Action callback)
    {
        yield return null;
        callback?.Invoke();
    }
}
