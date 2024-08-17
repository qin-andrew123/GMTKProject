using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private GameObject extensionPrefab;
    private float yHeight = 0f;
    private Vector2 currentCenter;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = extensionPrefab.GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            return;
        }
        yHeight = spriteRenderer.bounds.size.y;
        currentCenter = gameObject.transform.position;
        ExtendLadder();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ExtendLadder();
        }
    }
    private void ExtendLadder()
    {
        GameObject ladderSegment = Instantiate(extensionPrefab);
        ladderSegment.transform.position = currentCenter;
        Vector2 extensionPosition = currentCenter + new Vector2(0, yHeight);
        currentCenter = extensionPosition;
    }
}
