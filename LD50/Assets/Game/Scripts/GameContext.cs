using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

[DefaultExecutionOrder(-1)]
public class GameContext : MonoBehaviour
{
    [SerializeField] private Monologue monologue;
    public Monologue Monologue => monologue;

    [SerializeField] private Typing typing;
    public Typing Typing => typing;

    [SerializeField] private QuitController quitController;
    public QuitController QuitController => quitController;

    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1000, 10);
    }
}
