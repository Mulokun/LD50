using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private EffectData effectTest;

    private List<EffectHandle> effectList = new List<EffectHandle>();

    public void CreateEffect(EffectData data)
    {
        EffectHandle effect = new EffectHandle(data, gameSystem);
        effectList.Add(effect);
    }

    private void Update()
    {
        foreach (EffectHandle e in effectList)
        {
            e.Update();
        }

        int i = 0;
        while (i < effectList.Count)
        {
            if (effectList[i].EffectData.IsKilled)
            {
                effectList.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
}
