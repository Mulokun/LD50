using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class World : MonoBehaviour
{
    [SerializeField] private Camera currentCamera;
    [SerializeField] private WorldCell cellPrefab;
    [SerializeField] private Vector2 worldSize;
    [Title("Colliders")]
    [SerializeField] private BoxCollider upCollider;
    [SerializeField] private BoxCollider downCollider;
    [SerializeField] private BoxCollider leftCollider;
    [SerializeField] private BoxCollider rightCollider;

    private List<WorldCell> cellList = new List<WorldCell>();
    public List<WorldCell> CellList => cellList;

    private void Awake()
    {
        currentCamera.orthographicSize = Mathf.FloorToInt(worldSize.y * 0.5f);

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.x; y++)
            {
                WorldCell cell = Instantiate<WorldCell>(cellPrefab, transform);
                cell.transform.localPosition = new Vector3(x - Mathf.FloorToInt(worldSize.x * 0.5f), 0f, y - Mathf.FloorToInt(worldSize.y * 0.5f));
                cellList.Add(cell);
            }
        }

        float larger = 10;
        upCollider.size = new Vector3(worldSize.x + larger, 4, larger + 1);
        upCollider.transform.localPosition = new Vector3(0, 0, -Mathf.CeilToInt(worldSize.y * 0.5f) - larger * 0.5f);
        downCollider.size = new Vector3(worldSize.x + larger, 4, larger + 1);
        downCollider.transform.localPosition = new Vector3(0, 0, Mathf.CeilToInt(worldSize.y * 0.5f) + larger * 0.5f);
        leftCollider.size = new Vector3(larger + 1, 4, worldSize.y + larger);
        leftCollider.transform.localPosition = new Vector3(-Mathf.CeilToInt(worldSize.y * 0.5f) - larger * 0.5f, 0, 0);
        rightCollider.size = new Vector3(larger + 1, 4, worldSize.y + larger);
        rightCollider.transform.localPosition = new Vector3(Mathf.CeilToInt(worldSize.y * 0.5f) + larger * 0.5f, 0, 0);
    }
}
