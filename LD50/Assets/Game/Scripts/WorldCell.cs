using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    private Tween tweenScale;
    private Tween tweenColor;

    private List<EffectHandle> effectList = new List<EffectHandle>();
    private EffectHandle currentEffect = null;
    public bool HasEffects => effectList.Count > 0;

    private void Awake()
    {
        UpdateEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EffectHandle>(out EffectHandle effect))
        {
            effectList.Add(effect);
            UpdateEffect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EffectHandle>(out EffectHandle effect))
        {
            if (effectList.Remove(effect))
            {
                UpdateEffect();
            }
        }
    }

    private void UpdateEffect()
    {
        if (effectList.Count > 0 && currentEffect != effectList[0])
        {
            currentEffect = effectList[0];

            tweenScale?.Kill();
            tweenColor = mesh.material.DOColor(currentEffect.EffectData.Colors.Evaluate(0), 0.3f);
            transform.DOScaleY(2f, 0.3f).OnComplete(() =>
            {
                tweenScale = transform.DOScaleY(2f + Random.Range(0.05f, 0.2f), Random.Range(1f, 2f)).SetLoops(-1, LoopType.Yoyo);
            });
        }
        else
        {
            currentEffect = null;

            tweenScale?.Kill();
            tweenColor = mesh.material.DOColor(new Color(50f / 255f, 50f / 255f, 50f / 255f), 0.3f);
            transform.DOScaleY(1f, 0.3f).OnComplete(() =>
            {
                tweenScale = transform.DOScaleY(1f + Random.Range(0.2f, 0.5f), Random.Range(0.5f, 1f)).SetLoops(-1, LoopType.Yoyo);
            });
        }
    }
}
