using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    private void Awake()
    {
        mesh.transform.DOLocalMoveY(Random.Range(0.05f, 0.2f), Random.Range(1f, 2f)).SetLoops(-1, LoopType.Yoyo);
    }
}
