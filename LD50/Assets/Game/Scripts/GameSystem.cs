using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private World world;
    public World World => world;

    [SerializeField] private EffectSystem effectSystem;
    public EffectSystem EffectSystem => effectSystem;

    [SerializeField] private CharacterMovement characterMovement;
    public CharacterMovement CharacterMovement => characterMovement;

    [SerializeField] private CharacterHealth characterHealth;
    public CharacterHealth CharacterHealth => characterHealth;

    [SerializeField] private TMP_Text timerText;

    public void SetMovementActive(bool value)
    {
        characterMovement.enabled = value;
    }

    public void SetTimerActive(bool value)
    {
        timerText.gameObject.SetActive(value);
    }

    public void UpdateTimerText(float time)
    {
        timerText.text = $"Timer\n{time:F1}";
    }
}
