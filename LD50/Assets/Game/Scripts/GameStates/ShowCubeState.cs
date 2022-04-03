using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_state_cube", menuName = "Game State/New State : Show cube", order = 0)]
public class ShowCubeState : GameState
{
    public GameState nextState;
    [Range(1, 10)] public float ZoomValue;
    [Range(0.1f, 2f)] public float ZoomInDuration;
    public bool ZoomToDefault;
    [Range(0.1f, 2f)] public float ZoomOutDuration;
    private Tween rotationTween;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;

        context.MainCamera.DOOrthoSize(ZoomValue, ZoomInDuration).SetEase(Ease.OutCirc);

        rotationTween = context.GameSystem.CharacterMovement.transform.DOLocalRotate(context.GameSystem.CharacterMovement.transform.rotation.eulerAngles + Vector3.up, 0.1f).SetLoops(-1, LoopType.Incremental);

        context.QuitController.OnQuitAttemptTrigger += EndofState;

        yield return new WaitForSeconds(ZoomInDuration);
    }

    public override void Update()
    {

    }

    private void EndofState()
    {
        context.QuitController.OnQuitAttemptTrigger -= EndofState;
        context.GameFlow.SwitchState(this, nextState);
    }

    public override IEnumerator Coroutine_Exit()
    {
        rotationTween.Kill(false);
        context.GameSystem.CharacterMovement.transform.DOLocalRotateQuaternion(Quaternion.identity, ZoomOutDuration);

        if (ZoomToDefault)
        {
            context.MainCamera.DOOrthoSize(context.GameSystem.World.OrthographicSize, ZoomOutDuration).SetEase(Ease.InQuart);
            yield return new WaitForSeconds(ZoomOutDuration);
        }
    }
}
