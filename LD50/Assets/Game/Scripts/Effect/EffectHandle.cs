using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandle
{
    public EffectData EffectData { get; private set; }
    private GameSystem gameSystem;
    private World world => gameSystem.World;

    private Vector2 position;

    public EffectHandle(EffectData data, GameSystem gs)
    {
        EffectData = data;
        gameSystem = gs;

        Spawn();
    }

    private void Spawn()
    {
        position = EffectData.SpawnPosition(world);
    }

    private void Despawn()
    {
        for (int i = 0; i < world.CellList.Count; i++)
        {
            WorldCell c = world.CellList[i];
            c.RemoveEffect(this);
        }
    }

    public void Update()
    {
        if(EffectData.IsKilled)
        {
            Despawn();
            return;
        }

        position = EffectData.UpdatePosition(position, gameSystem);

        for (int i = 0; i < world.CellList.Count; i++)
        {
            WorldCell c = world.CellList[i];
            Vector2 cpos = new Vector2(c.transform.position.x, c.transform.position.z);

            bool isTriggered = EffectData.IsTrigger(position, cpos);
            bool isAffected = c.IsAffectedBy(this);

            if(isTriggered && !isAffected)
            {
                c.AddEffect(this);
            }
            else if(!isTriggered && isAffected)
            {
                if(EffectData.PersistantDuration > 0)
                {
                    c.RemoveEffect(this, EffectData.PersistantDuration);
                }
                else
                {
                    c.RemoveEffect(this);
                }
            }
        }
    }
}
