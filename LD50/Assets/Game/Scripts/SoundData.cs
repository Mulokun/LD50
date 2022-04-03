using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_sounds data", menuName = "Datas/New Sounds Data", order = 0)]
public class SoundData : ScriptableObject
{
    public AudioClip[] sounds;

    public AudioClip GetRandom()
    {
        return sounds[Random.Range(0, sounds.Length)];
    }
}
