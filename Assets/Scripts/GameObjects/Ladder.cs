using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ladder : MonoBehaviour
{
    [SerializeField] private GameObject extensionPrefab;
    [SerializeField] private CameraControlTrigger LookUpTrigger;
    [SerializeField] private int numLadderPerExtension;
    [SerializeField] private AudioClip extensionSFX;
    private float yHeight = 0f;
    private Vector2 currentCenter;
    private bool bIsFirstTime = true;
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
        GlobalData.OnClearLevel += ExtendLadder;
        ExtendLadder();
        bIsFirstTime = false;
    }

    private void OnDestroy()
    {
        GlobalData.OnClearLevel -= ExtendLadder;
    }
    private void ExtendLadder()
    {
        if (!bIsFirstTime)
        {
            GlobalData.Instance.playerReference.GetComponent<AudioSource>().PlayOneShot(extensionSFX);
            numLadderPerExtension /= 2;
        }
        for (int i = 0; i < numLadderPerExtension; i++)
        {
            GameObject ladderSegment = Instantiate(extensionPrefab);
            ladderSegment.transform.position = currentCenter;
            Vector2 extensionPosition = currentCenter + new Vector2(0, yHeight);
            currentCenter = extensionPosition;
            LookUpTrigger.transform.position = currentCenter;
        }

    }
}
