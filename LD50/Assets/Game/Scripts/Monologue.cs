using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class Monologue : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private TMP_Text monologueLine;
    [SerializeField] private LinesData lines;
    private InputAction actionNext;
    private Tween displayCharacterTween;

    public delegate void OnCompleteTypingEvent();
    public event OnCompleteTypingEvent OnCompleteTrigger;

    private void Awake()
    {
        actionNext = inputAsset.FindAction("Game/NextLine");
        actionNext.canceled += NextLine;
        actionNext.Enable();

        lines.Reset();
        NextLine();
    }

    private void OnDisable()
    {
        actionNext.Disable();
    }

    private void NextLine(InputAction.CallbackContext context)
    {
        NextLine();
    }

    private void NextLine()
    {
        if(displayCharacterTween != null && displayCharacterTween.IsActive() && displayCharacterTween.IsPlaying())
        {
            displayCharacterTween.Complete();
        }
        else if (lines.TryGetNextLine(out string line))
        {
            monologueLine.text = line;
            monologueLine.maxVisibleCharacters = 0;
            displayCharacterTween = monologueLine.DOMaxVisibleCharacters(line.Length, 0.1f * line.Length).From(0);
        }
        else
        {
            OnCompleteTrigger?.Invoke();
        }
    }
}
