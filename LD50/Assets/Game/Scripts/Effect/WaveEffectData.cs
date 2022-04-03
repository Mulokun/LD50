using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_effect", menuName = "Datas/New Effect - Wave", order = 0)]
public class WaveEffectData : EffectData
{
    protected Vector2 direction { get; set; }

    public override bool IsTrigger(Vector2 effectPosition, Vector2 cellPosition)
    {
        Vector2 op = new Vector2(Mathf.Abs(direction.y), Mathf.Abs(direction.x)) * Radius;
        Rect r = new Rect(effectPosition - (op * 0.5f), Vector2.one + op);
        return r.Contains(cellPosition);
    }

    public override Vector2 UpdatePosition(Vector2 currentPosition, GameSystem gameSystem)
    {
        return currentPosition + direction * SpeedMovement * Time.deltaTime;
    }

    public override Vector2 SpawnPosition(World w)
    {
        int orientation = Random.Range(0, 4);
        float pl = Random.value;
        Vector2 final = Vector2.zero;
        switch(orientation)
        {
            case 0:
                final = new Vector2(w.Size.x, Mathf.Lerp(-w.Size.y, w.Size.y, pl));
                direction = Vector2.left + Vector2.up;
                break;
            case 1:
                final = new Vector2(-w.Size.x, Mathf.Lerp(-w.Size.y, w.Size.y, pl));
                direction = Vector2.right + Vector2.down;
                break;
            case 2:
                final = new Vector2(Mathf.Lerp(-w.Size.x, w.Size.x, pl), w.Size.y);
                direction = Vector2.down + Vector2.left;
                break;
            case 3:
            default:
                final = new Vector2(Mathf.Lerp(-w.Size.x, w.Size.x, pl), -w.Size.y);
                direction = Vector2.up + Vector2.right;
                break;
        }

        final *= 0.5f;
        float d = final.magnitude;
        float a = Mathf.Deg2Rad * (Vector2.SignedAngle(Vector2.right, final) - 45);
        final.x = Mathf.Cos(a) * d;
        final.y = Mathf.Sin(a) * d;

        direction.Normalize();

        return final;
    }
}
