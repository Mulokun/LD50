using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new_effect", menuName = "Datas/New Effect", order = 0)]
public class EffectData : ScriptableObject
{
    public float Radius;
    [Title("Effect on Cell")]
    public Gradient Colors;
    [MinMaxSlider(1, 3)] public Vector2 Scale;
    public float RandomScale;
    public float Speed;
}
