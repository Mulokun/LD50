using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class Typing : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private TMP_Text lineToType;
    [SerializeField] private LinesData lines;
    [SerializeField] private GameObject enterObject;
    private int maxCharacter = 0;
    private int currentVisibleCharacter = 0;
    private InputAction actionTyping;
    private InputAction actionEnter;

    public delegate void OnCompleteTypingEvent();
    public event OnCompleteTypingEvent OnCompleteTrigger;

    private void Awake()
    {
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

        lines.Reset();
        SetNewLine();
    }

    private void OnEnable()
    {
        actionTyping.Enable();
        actionEnter.Disable();
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
        }
        else
        {
            OnCompleteTrigger?.Invoke();
        }
    }

    private void ValidateLine(InputAction.CallbackContext context)
    {
        SetNewLine();

        actionTyping.Enable();
        actionEnter.Disable();
        enterObject.SetActive(false);
    }

    private void TypeCharacter(InputAction.CallbackContext context)
    {
        currentVisibleCharacter++;
        if (currentVisibleCharacter <= maxCharacter)
        {
            lineToType.maxVisibleCharacters = currentVisibleCharacter;
        }
        else
        {
            enterObject.SetActive(true);
            actionTyping.Disable();
            actionEnter.Enable();
        }
    }
}
