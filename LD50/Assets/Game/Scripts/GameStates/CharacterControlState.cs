using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_character_control", menuName = "Game State/New State : Character Control", order = 0)]
public class CharacterControlState : GameState
{
    public GameState timeOverNextState;
    public GameState DeathNextState;

    public GameRoundData RoundData;
    private float elaspedTime;
    private Sequence spawnSequence;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;

        float ratio = Mathf.Abs(context.MainCamera.orthographicSize - context.GameSystem.World.OrthographicSize) / context.GameSystem.World.OrthographicSize;
        context.MainCamera.DOOrthoSize(context.GameSystem.World.OrthographicSize, 0.3f * ratio).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(0.3f * ratio);

        context.GameSystem.SetMovementActive(true);
        context.GameSystem.SetTimerActive(true);
        elaspedTime = 0;

        context.GameSystem.CharacterHealth.OnDeathTrigger += OnDeath;

        yield return null;

        StartSpawnSequence();
    }

    public override void Update()
    {
        context.GameSystem.UpdateTimerText(Mathf.Max(0f, RoundData.Duration - elaspedTime));

        elaspedTime += Time.deltaTime;
        if (elaspedTime > RoundData.Duration)
        {
            context.GameFlow.SwitchState(this, timeOverNextState);
        }
    }

    private void OnDeath()
    {
        context.GameSystem.CharacterHealth.OnDeathTrigger -= OnDeath;
        context.GameFlow.SwitchState(this, DeathNextState);
    }

    private void StartSpawnSequence()
    {
        spawnSequence = DOTween.Sequence();
        foreach(GameRoundData.EffectSpawner s in RoundData.Spawner)
        {
            spawnSequence.InsertCallback(s.Time, () => context.GameSystem.EffectSystem.CreateEffect(s.Effect));
        }
        spawnSequence.Play();
    }

    public override IEnumerator Coroutine_Exit()
    {
        spawnSequence?.Kill();
        context.GameSystem.SetMovementActive(false);
        context.GameSystem.SetTimerActive(false);

        yield return null;
    }
}
