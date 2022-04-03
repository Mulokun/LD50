using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Typing : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private TMP_Text lineToType;
    private LinesData lines;
    [SerializeField] private GameObject enterObject;
    [SerializeField] private RectTransform caret;
    [SerializeField] private Image progress;
    [SerializeField] private TMP_Text validateText;
    private int maxCharacter = 0;
    private int currentVisibleCharacter = 0;
    private InputAction actionTyping;
    private InputAction actionEnter;

    [Title("Sound")]
    [SerializeField] private SoundData keySounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0.02f, 0.1f)] private float audioDelay;
    private float nextSoundTime = 0;

    private RectTransform rect;
    private Sequence validationSequence;

    public delegate void OnCompleteTypingEvent();
    public event OnCompleteTypingEvent OnCompleteTrigger;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        validateText.gameObject.SetActive(false);

        if (actionTyping == null)
        {
            actionTyping = inputAsset.FindAction("Game/TypeChar");
            actionTyping.performed += TypeCharacter;
        }
        if (actionEnter == null)
        {
            actionEnter = inputAsset.FindAction("Game/TypeValidate");
            actionEnter.started += ValidateLine;
        }
    }

    public void Initialize(LinesData l)
    {
        enterObject.SetActive(false);
        caret.gameObject.SetActive(false);

        lines = l;
        progress.fillAmount = 0;
        lines.Reset();
        SetNewLine();
    }

    private void OnEnable()
    {
        // actionTyping.Enable();
        actionEnter.Disable();

        CreateValidationSequence();
        rect.DOAnchorMin(new Vector2(0.1f, 0.5f), 0.3f).SetEase(Ease.OutQuad).From(new Vector2(0.1f, 0.7f)).OnComplete(() =>
        {
            actionTyping.Enable();
            caret.gameObject.SetActive(true);
        });
    }

    private void OnDisable()
    {
        actionTyping.Disable();
        actionEnter.Disable();
    }

    private void SetNewLine()
    {
        if (lines.TryGetNextLine(out string line))
        {
            lineToType.text = line;
            maxCharacter = lineToType.text.Length;
            lineToType.maxVisibleCharacters = 0;
            currentVisibleCharacter = 0;
            CaretUpdate();
            ProgressUpdate();
        }
        else
        {
            actionTyping.Disable();
            actionEnter.Disable();
            //OnCompleteTrigger?.Invoke();
            validationSequence.Play();
        }
    }

    private void ValidateLine(InputAction.CallbackContext context)
    {
        enterObject.SetActive(false);
        SetNewLine();

        actionTyping.Enable();
        actionEnter.Disable();
    }

    private void TypeCharacter(InputAction.CallbackContext context)
    {
        currentVisibleCharacter++;
        if (currentVisibleCharacter <= maxCharacter)
        {
            lineToType.maxVisibleCharacters = currentVisibleCharacter;
            CaretUpdate();
            PlaySound();
        }
        else
        {
            enterObject.SetActive(true);
            actionTyping.Disable();
            actionEnter.Enable();
        }
    }

    private void CaretUpdate()
    {
        if(lineToType?.textInfo?.characterInfo == null)
        {
            return;
        }

        Vector3 p;
        if (currentVisibleCharacter < lineToType.textInfo.characterInfo.Length)
        {
            p = lineToType.textInfo.characterInfo[currentVisibleCharacter].bottomLeft;
        }
        else
        {
            p = lineToType.textInfo.characterInfo[lineToType.textInfo.characterInfo.Length - 1].bottomRight;
            p.x += 3;
        }

        p.y -= 5;
        caret.localPosition = p;
    }

    private void ProgressUpdate()
    {
        float amount = (float)(lines.CurrentLine - 1) / (float)lines.Lines.Length;
        progress.DOFillAmount(amount, 0.3f).SetEase(Ease.OutQuad);
    }

    private void CreateValidationSequence()
    {
        validationSequence = DOTween.Sequence();
        validationSequence.AppendCallback(() => lineToType.text = string.Empty);
        validationSequence.AppendCallback(() => caret.gameObject.SetActive(false));
        validationSequence.Append(progress.DOFillAmount(1f, 0.3f).SetEase(Ease.OutQuad));
        validationSequence.AppendCallback(() => enterObject.SetActive(false));
        validationSequence.AppendCallback(() => validateText.gameObject.SetActive(true));
        validationSequence.AppendInterval(1f);
        validationSequence.AppendCallback(() => validateText.gameObject.SetActive(false));
        validationSequence.Append(rect.DOAnchorMin(new Vector2(0.1f, 0.7f), 0.3f).SetEase(Ease.OutQuad));
        validationSequence.AppendCallback(() => OnCompleteTrigger?.Invoke());
    }

    private void PlaySound()
    {
        if(nextSoundTime <= Time.time)
        {
            audioSource.PlayOneShot(keySounds.GetRandom(), 0.6f);
            nextSoundTime = Time.time + audioDelay;
        }
    }
}
