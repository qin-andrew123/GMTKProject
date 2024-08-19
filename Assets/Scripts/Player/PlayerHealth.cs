using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider healthSliderUI;
    private int numInvulnSaves = 0;
    private bool bCanTakeDamage = true;
    public bool CanTakeDamage
    {
        get { return bCanTakeDamage; }
    }
    private int maxHealth;
    private int currentHealth;
    private float invulnTime; 
    private void InitializeStats()
    {

        currentHealth = GlobalData.Instance.GetInitMaxHealth();
        maxHealth = GlobalData.Instance.GetInitMaxHealth();
        invulnTime = GlobalData.Instance.GetInvulnerabilityTime();
        if (!healthSliderUI)
        {
            Debug.LogError("No reference for health slider ui. make sure to set it");
            return;
        }

        healthSliderUI.maxValue = GlobalData.Instance.GetInitMaxHealth();
        healthSliderUI.value = currentHealth;
    }

    void Start()
    {
        InitializeStats();
        Player player = GetComponent<Player>(); 
        if(player)
        {
            numInvulnSaves = player.ObstacleImmunityLevel;
        }

        Player.OnUpgradePurchased += UpdateMaxHealth;
        Player.OnUpgradePurchased += UpdateImmunities;
    }
    private void OnDestroy()
    {
        Player.OnUpgradePurchased -= UpdateMaxHealth;
        Player.OnUpgradePurchased -= UpdateImmunities;
    }
    private void UpdateMaxHealth(Upgrade type, int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    private void UpdateImmunities(Upgrade type, int amount)
    {
        numInvulnSaves = amount;
    }
    public void UpdateHealthUI()
    {
        if(healthSliderUI.maxValue != maxHealth)
        {
            healthSliderUI.maxValue = maxHealth;
        }
        healthSliderUI.value = currentHealth;
    }

    public void ResetInvulnSaves()
    {
        Player player = GetComponent<Player>();
        numInvulnSaves = player.ObstacleImmunityLevel;
    }
    public void AdjustHealth(int inputAmount)
    {
        if(inputAmount < 0)
        {
            if(numInvulnSaves > 0)
            {
                --numInvulnSaves;
                StartCoroutine(InvulnerabilityTime());
                return;
            }
        }
        
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
