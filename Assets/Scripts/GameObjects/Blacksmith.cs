using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        PlayerController.PlayerAttemptShop += QueryNearBlacksmith;  
    }
    private void OnDestroy()
    {
        PlayerController.PlayerAttemptShop -= QueryNearBlacksmith;
    }
    private void QueryNearBlacksmith(GameObject player)
    {
        Debug.Log("Running QNB");
        if (!player)
        {
            Debug.Log("Player is null uhoh");
            return;
        }
        gameObject.SetActive(!gameObject.activeSelf);
        Debug.Log("Running QNB SetActive");

    }
}
