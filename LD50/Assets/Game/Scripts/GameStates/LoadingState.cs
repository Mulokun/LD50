using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_loading", menuName = "Game State/New State : Loading", order = 0)]
public class LoadingState : GameState
{
    public GameState NextState;

    [Range(0f, 10f)] public float LoadingDuration;
    private float elaspedTime;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;
        context.LoadingScreen.SetActive(true);
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
    }

    public override IEnumerator Coroutine_Exit()
    {
        context.LoadingScreen.SetActive(false);
        yield return null;
    }
}
