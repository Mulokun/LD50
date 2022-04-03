using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private UIHealthGauge HealthGauge;
    [SerializeField, Range(1f, 1000f)] private float HealthMax;
    private float currentHealth = 100f;
    [SerializeField, Range(0.3f, 3f)] private float HealingTiming;

    private Sequence callbackHealing;
    private List<WorldCell> cellList = new List<WorldCell>();

    public delegate void OnDeathEvent();
    public event OnDeathEvent OnDeathTrigger;

    private void Awake()
    {
        callbackHealing = DOTween.Sequence();
        callbackHealing.InsertCallback(HealingTiming, Heal);
        callbackHealing.SetAutoKill(false);

        Heal();
    }

    private void Damage(float value)
    {
        currentHealth -= value;
        HealthGauge.SetValue(currentHealth / HealthMax);
        if(currentHealth <= 0)
        {
            OnDeathTrigger?.Invoke();
        }
        else
        {
            callbackHealing.Restart();
        }
    }

    private void FixedUpdate()
    {
        foreach (WorldCell c in cellList)
        {
            if(c.HasEffects)
            {
                Damage(1f);
                break;
            }
        }
    }

    private void Heal()
    {
        currentHealth = HealthMax;
        HealthGauge.SetValue(currentHealth / HealthMax);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WorldCell>(out WorldCell cell))
        {
            cellList.Add(cell);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WorldCell>(out WorldCell cell))
        {
            cellList.Remove(cell);
        }
    }
}
