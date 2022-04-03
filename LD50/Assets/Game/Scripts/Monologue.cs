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
    private InputAction actionNext = null;

    [SerializeField] private TMP_Text monologueLine;
    private LinesData lines;

    [Title("Sounds")]
    [SerializeField] private SoundData syllableSound;
    [SerializeField] private AudioSource audioSource;

    private CanvasGroup canvas;

    private Tween displayCharacterTween;
    private Sequence currentCharacterSequence;

    public delegate void OnCompleteTypingEvent();
    public event OnCompleteTypingEvent OnCompleteTrigger;

    public void Awake()
    {
        canvas = GetComponent<CanvasGroup>();

        if (actionNext == null)
        {
            actionNext = inputAsset.FindAction("Game/NextLine");
            actionNext.canceled += NextLine;
        }
    }

    public void Initialize(LinesData l)
    {
        lines = l;
        lines.Reset();
    }

    private void OnEnable()
    {
        canvas.DOFade(1, 0.2f).From(0);
        actionNext.Enable();
    }

    private void OnDisable()
    {
        actionNext.Disable();
    }

    private void NextLine(InputAction.CallbackContext context)
    {
        NextLine();
    }

    public void NextLine()
    {
        if(displayCharacterTween != null && displayCharacterTween.IsActive() && displayCharacterTween.IsPlaying())
        {
            audioSource.mute = true;
            displayCharacterTween.Kill(true);
            if (currentCharacterSequence != null && currentCharacterSequence.IsActive() && currentCharacterSequence.IsPlaying())
            {
                currentCharacterSequence.Kill();
            }
            audioSource.mute = false;
        }
        else if (lines.TryGetNextLine(out string line))
        {
            monologueLine.text = line;
            monologueLine.maxVisibleCharacters = 0;

            int lettersPerSound = 3;
            float SecondsPerLetter = 0.03f;
            displayCharacterTween = monologueLine.DOMaxVisibleCharacters(line.Length, SecondsPerLetter * line.Length).From(0);

            currentCharacterSequence = DOTween.Sequence();
            for (int i = 0; i < line.Length / lettersPerSound; i++)
            {
                currentCharacterSequence.InsertCallback(i * SecondsPerLetter * lettersPerSound,() => audioSource.PlayOneShot(syllableSound.GetRandom()));
            }
            currentCharacterSequence.Play();
        }
        else
        {
            monologueLine.text = string.Empty;

            canvas.DOFade(0, 0.2f).OnComplete(() => OnCompleteTrigger?.Invoke());
        }
    }
}
