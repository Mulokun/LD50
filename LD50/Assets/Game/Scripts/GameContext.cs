using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

[DefaultExecutionOrder(-1)]
public class GameContext : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public Camera MainCamera => mainCamera;

    [SerializeField] private GameObject loadingScreen;
    public GameObject LoadingScreen => loadingScreen;

    [SerializeField] private GameFlow gameFlow;
    public GameFlow GameFlow => gameFlow;

    [SerializeField] private Monologue monologue;
    public Monologue Monologue => monologue;

    [SerializeField] private Typing typing;
    public Typing Typing => typing;

    [SerializeField] private QuitController quitController;
    public QuitController QuitController => quitController;

    [SerializeField] private GameSystem gameSystem;
    public GameSystem GameSystem => gameSystem;

    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(5000, 10);
    }
}
