using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_character_control", menuName = "Game State/New State : Character Control", order = 0)]
public class CharacterControlState : GameState
{
    public GameState nextState;

    [Range(0, 300)] public int Duration;
    private float elaspedTime;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;
        context.GameSystem.SetMovementActive(true);
        context.GameSystem.SetTimerActive(true);
        elaspedTime = 0;

        yield return null;
    }

    public override void Update()
    {
        context.GameSystem.UpdateTimerText(Mathf.Max(0f, Duration - elaspedTime));

        elaspedTime += Time.deltaTime;
        if (elaspedTime > Duration)
        {
            context.GameFlow.SwitchState(this, nextState);
        }
    }

    public override IEnumerator Coroutine_Exit()
    {
        context.GameSystem.SetMovementActive(false);
        context.GameSystem.SetTimerActive(false);

        yield return null;
    }
}
