using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_monologue", menuName = "Game State/New State : Monologue", order = 0)]
public class MonologueState : GameState
{
    public GameState NextState;
    public LinesData MonologueLines;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;

        context.Monologue.Initialize(MonologueLines);
        context.Monologue.OnCompleteTrigger += EndofState;

        yield return null;

        context.Monologue.gameObject.SetActive(true);

        context.Monologue.NextLine();
    }

    public override void Update()
    {

    }

    private void EndofState()
    {
        context.Monologue.OnCompleteTrigger -= EndofState;
        context.GameFlow.SwitchState(this, NextState);
    }

    public override IEnumerator Coroutine_Exit()
    {
        context.Monologue.gameObject.SetActive(false);

        yield return null;
    }
}
