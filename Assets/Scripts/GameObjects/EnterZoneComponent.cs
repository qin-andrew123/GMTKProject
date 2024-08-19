using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterZoneComponent : MonoBehaviour
{
    [Tooltip("Set this layermask to the layers you want to get past this zone")]
    [SerializeField] private LayerMask permittedLayers;
    [Tooltip("Keep this as nothing. we want everything to get past except the layers from permitted")]
    [SerializeField] private LayerMask everythingExceptPermitted;
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        GlobalData.OnClearLevel += DenyPlayersReEntry;
        playerCollider = GetComponent<Collider2D>();
        playerCollider.excludeLayers = permittedLayers;
    }

    private void OnDestroy()
    {
        GlobalData.OnClearLevel -= DenyPlayersReEntry;
    }

    private void DenyPlayersReEntry()
    {
        playerCollider.excludeLayers = everythingExceptPermitted;
    }
}
