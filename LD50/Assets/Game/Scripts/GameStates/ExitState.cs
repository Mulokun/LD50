using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_ExitState", menuName = "Game State/New State : ExitState", order = 0)]
public class ExitState : GameState
{
    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;
        context.GameFlow.Exit(this);

        yield return null;
    }

    public override void Update()
    {

    }

    public override IEnumerator Coroutine_Exit()
    {
        yield return null;
    }
}
