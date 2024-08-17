using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    private int currentHealth;
    private int currentDamage;
    
    private void InitializeStats()
    {
        if(!stats)
        {
            return;
        }
    }
    void Start()
    {
        InitializeStats();
    }
}
