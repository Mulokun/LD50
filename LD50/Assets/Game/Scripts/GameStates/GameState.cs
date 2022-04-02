using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : ScriptableObject
{
    public abstract IEnumerator Coroutine_Start(GameContext  c);
    public abstract void Update();
    public abstract IEnumerator Coroutine_Exit();
}
