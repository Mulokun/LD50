using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;

[CreateAssetMenu(fileName = "new_state_loading", menuName = "Game State/New State : Loading", order = 0)]
public class LoadingState : GameState
{
    public GameState NextState;

    [TextArea]
    public string text;

    [Range(0f, 10f)] public float LoadingDuration;
    public bool KeyToSkip;
    private float elaspedTime;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;
        context.LoadingScreen.SetActive(true);
        context.LoadingScreen.GetComponentInChildren<TMP_Text>().text = text;
        elaspedTime = 0;

        yield return null;
    }

    public override void Update()
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime > LoadingDuration)
        {
            context.GameFlow.SwitchState(this, NextState);
        }

        if(KeyToSkip && Keyboard.current.anyKey.wasReleasedThisFrame)
        {
            context.GameFlow.SwitchState(this, NextState);
        }
    }

    public override IEnumerator Coroutine_Exit()
    {
        context.LoadingScreen.GetComponentInChildren<TMP_Text>().text = string.Empty;
        context.LoadingScreen.SetActive(false);
        yield return null;
    }
}
