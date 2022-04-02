using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitController : MonoBehaviour
{
    private int quitCounter = 0;

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
        return quitCounter >= 5;
    }

    private void Quit()
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
