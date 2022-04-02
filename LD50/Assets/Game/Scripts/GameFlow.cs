using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private GameContext context;

    [SerializeField] private GameState firstGameState;
    private GameState currentGameState;

    private void Start()
    {
        SwitchState(null, firstGameState);
    }

    private void Update()
    {
        currentGameState?.Update();
    }

    public void SwitchState(GameState from, GameState to)
    {
        StartCoroutine(Coroutine_Switch(from, to));
    }

    public void Exit(GameState from)
    {
        StartCoroutine(Coroutine_Switch(from, null));

        context.QuitController.IsQuittingAllowed = true;
        context.QuitController.Quit();
    }

    private IEnumerator Coroutine_Switch(GameState from, GameState to)
    {
        if (from != null)
        {
            yield return from.Coroutine_Exit();
            currentGameState = null;
        }

        yield return null;

        if (to != null)
        {
            yield return to.Coroutine_Start(context);
            currentGameState = to;
        }
    }
}
