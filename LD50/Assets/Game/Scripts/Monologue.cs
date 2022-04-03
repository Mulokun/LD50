using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Monologue : MonoBehaviour
{
    [System.Flags]
    public enum StagingEffect
    {
        CameraShaking
    }

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
    private DOTweenTMPAnimator animator = null;
    private List<Tween> characterTweenList = new List<Tween>();

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
        if(currentCharacterSequence != null && currentCharacterSequence.IsActive() && currentCharacterSequence.IsPlaying())
        {
            currentCharacterSequence.Kill(true);
            currentCharacterSequence = null;
        }
        else if (lines.TryGetNextLine(out string line))
        {
            SetAnimatedLineEffect(line);

            int lettersPerSound = 3;
            float SecondsPerLetter = 0.03f;

            if (currentCharacterSequence?.IsActive() ?? false)
            {
                currentCharacterSequence.Kill();
            }
            currentCharacterSequence = DOTween.Sequence();
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                if (animator.textInfo.characterInfo[i].isVisible)
                {
                    currentCharacterSequence.Join(animator.DOFadeChar(i, 1f, SecondsPerLetter).From(0f));
                }

                if (i % lettersPerSound == 0)
                {
                    currentCharacterSequence.InsertCallback(i * SecondsPerLetter, () => audioSource.PlayOneShot(syllableSound.GetRandom()));
                }
            }
            currentCharacterSequence.Play();
        }
        else
        {
            if (animator != null)
            {
                foreach (Tween t in characterTweenList)
                {
                    t.Kill();
                }
                characterTweenList.Clear();
                animator.Dispose();
                animator = null;
            }
            monologueLine.text = string.Empty;

            canvas.DOFade(0, 0.2f).OnComplete(() => OnCompleteTrigger?.Invoke());
        }
    }

    public void SetAnimatedLineEffect(string line)
    {
        if(animator != null)
        {
            foreach(Tween t in characterTweenList)
            {
                t.Kill();
            }
            characterTweenList.Clear();
            animator.Dispose();
            animator = null;
        }

        // string line = monologueLine.GetParsedText();

        string lineCleaned = line;
        lineCleaned = lineCleaned.Replace("[s]", string.Empty);
        lineCleaned = lineCleaned.Replace("[/s]", string.Empty);
        monologueLine.text = lineCleaned;

        int start = line.IndexOf('<');
        int end = line.IndexOf('>');
        while (start != -1 && end != -1)
        {
            line = line.Remove(start, end - start + 1);
            start = line.IndexOf('<');
            end = line.IndexOf('>');
        }

        animator = new DOTweenTMPAnimator(monologueLine);

        start = line.IndexOf("[s]");
        end = line.IndexOf("[/s]");
        if(start != -1 && end != -1)
        {
            line = line.Remove(end, 4);
            line = line.Remove(start, 3);
            end -= 4;
            for (int i = start; i <= end; i++)
            {
                if (!animator.textInfo.characterInfo[i].isVisible)
                {
                    continue;
                }
                Tween t = animator.DOOffsetChar(i, Vector3.up * 10f, 0.3f).SetDelay(0.1f * (i - start)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                characterTweenList.Add(t);
            }
        }
    }
}
