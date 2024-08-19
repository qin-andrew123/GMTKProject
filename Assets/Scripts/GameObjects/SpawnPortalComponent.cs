using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortalComponent : MonoBehaviour
{
    [SerializeField] private Transform desiredSpawnPoint;
    private bool bIsActivated = false;
    public static event Action<float> OnTravellingToSpawn;
    void Start()
    {
        GlobalData.OnClearLevel += ActivateReturnPortal;
        PlayerHealth.OnPlayerDie += ReturnPlayerToSpawn;
    }

    private void OnDestroy()
    {
        GlobalData.OnClearLevel -= ActivateReturnPortal;
        PlayerHealth.OnPlayerDie -= ReturnPlayerToSpawn;
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
        OnTravellingToSpawn?.Invoke(1.5f);
        CameraManager.Instance.UpdateMainCamera(0);
    }
}
