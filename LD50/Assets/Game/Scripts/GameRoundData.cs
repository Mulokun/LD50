using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_game_data", menuName = "Datas/New Game Round Data", order = 0)]
public class GameRoundData : ScriptableObject
{
    [System.Serializable]
    public struct EffectSpawner
    {
        public float Time;
        public EffectData Effect;
    }

    public float Duration;
    [TableList]
    public List<EffectSpawner> Spawner;
}
