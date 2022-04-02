using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_typing", menuName = "Game State/New State : Typing", order = 0)]
public class TypingState : GameState
{
    public GameState NextState;
    public LinesData TypingLines;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;

        context.Typing.Initialize(TypingLines);
        context.Typing.gameObject.SetActive(true);
        context.Typing.OnCompleteTrigger += EndofState;

        yield return null;
    }

    public override void Update()
    {

    }

    private void EndofState()
    {
        context.Typing.OnCompleteTrigger -= EndofState;
        context.GameFlow.SwitchState(this, NextState);
    }

    public override IEnumerator Coroutine_Exit()
    {
        context.Typing.gameObject.SetActive(false);

        yield return null;
    }
}
