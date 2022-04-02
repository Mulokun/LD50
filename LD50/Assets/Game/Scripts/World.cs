using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Vector2 worldSize;

    private void Awake()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.x; y++)
            {
                GameObject cell = Instantiate<GameObject>(cellPrefab, transform);
                cell.transform.localPosition = new Vector3(x - Mathf.FloorToInt(worldSize.x * 0.5f), 0f, y - Mathf.FloorToInt(worldSize.y * 0.5f));
            }
        }
    }
}
