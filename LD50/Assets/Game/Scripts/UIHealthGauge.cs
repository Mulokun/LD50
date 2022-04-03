using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHealthGauge : MonoBehaviour
{
    [SerializeField] private Image gauge;

    public void SetValue(float amount)
    {
        gauge.fillAmount = amount;
    }
}
