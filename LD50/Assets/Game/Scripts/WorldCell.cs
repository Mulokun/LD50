using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    private List<EffectHandle> effectList = new List<EffectHandle>();
    private EffectHandle currentEffect = null;
    public bool HasEffects => effectList.Count > 0;

    private Dictionary<EffectHandle, Coroutine> timedRemovingEffect = new Dictionary<EffectHandle, Coroutine>();

    private void Awake()
    {
        UpdateEffect();
    }

    public bool IsAffectedBy(EffectHandle e)
    {
        return effectList.Contains(e) && !timedRemovingEffect.ContainsKey(e);
    }

    public void AddEffect(EffectHandle e)
    {
        if (timedRemovingEffect.ContainsKey(e))
        {
            StopCoroutine(timedRemovingEffect[e]);
            timedRemovingEffect.Remove(e);
        }
        else
        {
            effectList.Add(e);
            UpdateEffect();
        }
    }

    public void RemoveEffect(EffectHandle e)
    {
        if (timedRemovingEffect.ContainsKey(e))
        {
            StopCoroutine(timedRemovingEffect[e]);
            timedRemovingEffect.Remove(e);
        }

        if (effectList.Remove(e))
        {
            UpdateEffect();
        }
    }

    public void RemoveEffect(EffectHandle e, float timer)
    {
        if(timedRemovingEffect.ContainsKey(e))
        {
            StopCoroutine(timedRemovingEffect[e]);
            timedRemovingEffect.Remove(e);
        }

        Coroutine c = StartCoroutine(Coroutine_RemoveEffect(e, timer));
        timedRemovingEffect.Add(e, c);
    }

    private IEnumerator Coroutine_RemoveEffect(EffectHandle e, float timer)
    {
        yield return new WaitForSeconds(timer);

        timedRemovingEffect.Remove(e);
        RemoveEffect(e);
    }

    private void UpdateEffect()
    {
        if (effectList.Count > 0 && currentEffect != effectList[0])
        {
            currentEffect = effectList[0];

            mesh.material.DOKill();
            mesh.material.DOColor(currentEffect.EffectData.Colors.Evaluate(Random.value), 0.2f);

            transform.DOKill();
            transform.DOScaleY(currentEffect.EffectData.RandomizedScale, 0.1f).OnComplete(() =>
            {
                transform.DOScaleY(currentEffect.EffectData.RandomizedScale, currentEffect.EffectData.RandomizedVariationSpeed).SetLoops(-1, LoopType.Yoyo);
            });
        }
        else
        {
            currentEffect = null;

            mesh.material.DOKill();
            mesh.material.DOColor(new Color(50f / 255f, 50f / 255f, 50f / 255f), 0.3f);

            transform.DOKill();
            transform.DOScaleY(1f, 0.1f).OnComplete(() =>
            {
                transform.DOScaleY(1f + Random.Range(0.2f, 0.5f), Random.Range(0.5f, 1f)).SetLoops(-1, LoopType.Yoyo);
            });
        }
    }
}
