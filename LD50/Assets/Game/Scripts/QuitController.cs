using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitController : MonoBehaviour
{
    private int quitCounter = 0;
    public bool IsQuittingAllowed { get; set; } = false;

    public delegate void OnQuitAttemptEvent();
    public event OnQuitAttemptEvent OnQuitAttemptTrigger;

    private void Awake()
    {
        Application.wantsToQuit += CanQuit;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }
    }

    private bool CanQuit()
    {
        quitCounter++;
        if (IsQuittingAllowed || quitCounter >= 10)
        {
            return true;
        }
        else
        {
            OnQuitAttemptTrigger?.Invoke();
            return false;
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        if (CanQuit())
        {
            EditorApplication.ExitPlaymode();
        }
#else
        Application.Quit();
#endif
    }
}
