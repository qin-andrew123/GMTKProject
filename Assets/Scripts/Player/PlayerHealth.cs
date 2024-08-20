using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDie;
    public static event Action<int> OnPlayerHealthChanged;
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
    }

    void Start()
    {
        InitializeStats();
        Player player = GetComponent<Player>();
        if (player)
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
        OnPlayerHealthChanged?.Invoke(maxHealth);
    }
    private void UpdateImmunities(Upgrade type, int amount)
    {
        numInvulnSaves = amount;
    }
    public void ResetInvulnSaves()
    {
        Player player = GetComponent<Player>();
        numInvulnSaves = player.ObstacleImmunityLevel;
    }
    public void AdjustHealth(int inputAmount)
    {
        if (inputAmount < 0)
        {
            if (numInvulnSaves > 0)
            {
                --numInvulnSaves;
                StartCoroutine(InvulnerabilityTime(invulnTime));
                return;
            }
        }

        currentHealth += inputAmount;
        OnPlayerHealthChanged?.Invoke(currentHealth);
        StartCoroutine(InvulnerabilityTime(invulnTime));

        if (currentHealth <= 0)
        {
            OnPlayerDie?.Invoke();
            ResetPlayerHealth();
        }
    }
    private void ResetPlayerHealth()
    {
        currentHealth = maxHealth;
        OnPlayerHealthChanged?.Invoke(maxHealth);
        ResetInvulnSaves();
    }
    public void CallInvulnerability(float inputTime)
    {
        StartCoroutine(InvulnerabilityTime(inputTime));
    }
    IEnumerator InvulnerabilityTime(float invulnTime)
    {
        bCanTakeDamage = false;
        yield return new WaitForSeconds(invulnTime);
        bCanTakeDamage = true;
    }
}
