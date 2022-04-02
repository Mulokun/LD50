using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Monologue : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    private InputAction actionNext;

    [SerializeField] private TMP_Text monologueLine;
    [SerializeField] private LinesData lines;

    [Title("Sounds")]
    [SerializeField] private SyllableSoundData syllableSound;
    [SerializeField] private AudioSource audioSource;

    private Tween displayCharacterTween;
    private Sequence currentCharacterSequence;

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
        if(currentCharacterSequence != null && currentCharacterSequence.IsActive() && currentCharacterSequence.IsPlaying())
        {
            audioSource.mute = true;
            currentCharacterSequence.Complete();
            audioSource.mute = false;
        }
        else if (lines.TryGetNextLine(out string line))
        {
            monologueLine.text = line;
            // monologueLine.maxVisibleCharacters = 0;
            // displayCharacterTween = monologueLine.DOMaxVisibleCharacters(line.Length, 0.1f * line.Length).From(0);

            currentCharacterSequence = DOTween.Sequence();
            DOTweenTMPAnimator animator = new DOTweenTMPAnimator(monologueLine);
            for (int i = 0; i < animator.textInfo.characterCount; ++i)
            {
                if (!animator.textInfo.characterInfo[i].isVisible)
                {
                    continue;
                }

                if (i % 2 == 0)
                {
                    currentCharacterSequence.AppendCallback(() => audioSource.PlayOneShot(syllableSound.GetRandom()));
                    currentCharacterSequence.Join(animator.DOFadeChar(i, 1, 0.03f).From(0));
                }
                else
                {
                    currentCharacterSequence.Append(animator.DOFadeChar(i, 1, 0.03f).From(0));
                }
            }
            currentCharacterSequence.Play();
        }
        else
        {
            monologueLine.text = string.Empty;
            OnCompleteTrigger?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
