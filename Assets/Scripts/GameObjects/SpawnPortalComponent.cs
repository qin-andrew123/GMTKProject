using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortalComponent : MonoBehaviour
{
    [SerializeField] private Transform desiredSpawnPoint;
    private bool bIsActivated = false;
    public static event Action OnTravellingToSpawn;
    void Start()
    {
        GlobalData.OnClearLevel += ActivateReturnPortal;
    }

    private void OnDestroy()
    {
        GlobalData.OnClearLevel -= ActivateReturnPortal; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            if(bIsActivated)
            {
                ReturnPlayerToSpawn();
                bIsActivated = false;
            }
        }
    }
    private void ActivateReturnPortal()
    {
        bIsActivated = true;
    }
    private void ReturnPlayerToSpawn()
    {
        GlobalData.Instance.playerReference.transform.position = desiredSpawnPoint.position;
        OnTravellingToSpawn?.Invoke();
        CameraManager.Instance.UpdateConfiningShape(desiredSpawnPoint.position);
    }
}
