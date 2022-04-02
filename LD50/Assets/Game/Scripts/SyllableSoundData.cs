using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_syllabus", menuName = "Datas/New Syllable Sounds", order = 0)]
public class SyllableSoundData : ScriptableObject
{
    public AudioClip[] syllables;

    public AudioClip GetRandom()
    {
        return syllables[Random.Range(0, syllables.Length)];
    }
}
