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
    [Range(0.1f, 2f)] public float ZoomOutDuration;
    private float baseOthoSize = 0;
    private Tween rotationTween;

    private GameContext context;

    public override IEnumerator Coroutine_Start(GameContext c)
    {
        context = c;

        baseOthoSize = context.MainCamera.orthographicSize;
        context.MainCamera.DOOrthoSize(ZoomValue, ZoomInDuration).SetEase(Ease.OutCirc);

        rotationTween = context.CharacterMovement.transform.DOLocalRotate(context.CharacterMovement.transform.rotation.eulerAngles + Vector3.up, 0.1f).SetLoops(-1, LoopType.Incremental);

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
        context.CharacterMovement.transform.DOLocalRotateQuaternion(Quaternion.identity, ZoomOutDuration);

        context.MainCamera.DOOrthoSize(baseOthoSize, ZoomOutDuration).SetEase(Ease.InQuart);

        yield return new WaitForSeconds(ZoomOutDuration);
    }
}
