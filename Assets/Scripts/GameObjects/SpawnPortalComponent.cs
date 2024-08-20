using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortalComponent : MonoBehaviour
{
    [SerializeField] private Transform desiredSpawnPoint;
    public static event Action<float> OnTravellingToSpawn;
    void Start()
    {
        PlayerHealth.OnPlayerDie += ReturnPlayerToSpawn;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDie -= ReturnPlayerToSpawn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            ReturnPlayerToSpawn();
        }
    }
    private void ReturnPlayerToSpawn()
    {
        GlobalData.Instance.playerReference.transform.position = desiredSpawnPoint.position;
        OnTravellingToSpawn?.Invoke(1.5f);
        CameraManager.Instance.UpdateMainCamera(0);
    }
}
