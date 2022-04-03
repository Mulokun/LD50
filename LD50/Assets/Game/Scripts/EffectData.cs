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
    public float RandomizedScale => Random.Range(Scale.x, Scale.y);
    [MinMaxSlider(0.05f, 3f)] public Vector2 VariationSpeed;
    public float RandomizedVariationSpeed => Random.Range(VariationSpeed.x, VariationSpeed.y);
    [Range(0, 3)] public float PersistantDuration;
    [Range(0, 120)] public float DurationBeforeKill;
    [Range(0, 10)] public float SpeedMovement;

    public bool IsTrigger(Vector2 effectPosition, Vector2 cellPosition)
    {
        return (Vector2.Distance(effectPosition, cellPosition) <= Radius);
    }

    public Vector2 UpdatePosition(Vector2 currentPosition, GameSystem gameSystem)
    {
        Vector2 cpos = new Vector2(gameSystem.CharacterMovement.transform.position.x, gameSystem.CharacterMovement.transform.position.z);

        Vector2 direction = cpos - currentPosition;
        direction.Normalize();
        direction *= SpeedMovement * Time.deltaTime;
        return currentPosition + direction;
    }
}
