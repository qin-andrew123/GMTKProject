using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Slider healthSliderUI;
    
    private bool bCanTakeDamage = true;
    public bool CanTakeDamage
    {
        get { return bCanTakeDamage; }
    }
    private int currentHealth;
    private float invulnTime; 
    private void InitializeStats()
    {
        if (!stats)
        {
            return;
        }

        currentHealth = stats.maxHealth;
        invulnTime = stats.invulnerabilityTime;
        if (!healthSliderUI)
        {
            Debug.LogError("No reference for health slider ui. make sure to set it");
            return;
        }

        healthSliderUI.maxValue = stats.maxHealth;
        healthSliderUI.value = currentHealth;
    }

    void Start()
    {
        InitializeStats();
    }

    public void UpdateHealthUI()
    {
        healthSliderUI.value = currentHealth;
    }

    public void AdjustHealth(int inputAmount)
    {
        currentHealth += inputAmount;
        UpdateHealthUI();
        StartCoroutine(InvulnerabilityTime());
    }

    IEnumerator InvulnerabilityTime()
    {
        bCanTakeDamage = false;
        yield return new WaitForSeconds(invulnTime);
        bCanTakeDamage = true;
    }
}
