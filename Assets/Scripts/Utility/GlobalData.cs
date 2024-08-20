using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] public GameObject playerReference;
    public Dictionary<Upgrade, UpgradeData> upgradeDataDict = new Dictionary<Upgrade, UpgradeData>();
    public HashSet<Upgrade> purchasedUpgrades = new HashSet<Upgrade>();
    public static event Action OnCurrencyAmountChange;
    public static event Action OnClearLevel;
    public float GetPlayerInteractDistance()
    {
        return playerStats.interactDistance;
    }
    public CurrencyType GetCurrencyType()
    {
        return playerStats.currencyType;
    }
    public List<UpgradeData> GetUpgradeData()
    {
        return playerStats.playerUpgrades;
    }
    public int GetInitMaxHealth()
    {
        return playerStats.maxHealth;
    }
    public int GetCurrency()
    {
        return playerStats.numCurrency;
    }
    public float GetInvulnerabilityTime()
    {
        return playerStats.invulnerabilityTime;
    }
    public void AdjustCurrency(int amountToAdjust)
    {
        if (playerStats.numCurrency + amountToAdjust != playerStats.numCurrency)
        {
            OnCurrencyAmountChange?.Invoke();
        }
        playerStats.numCurrency += amountToAdjust;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        foreach (UpgradeData ud in playerStats.playerUpgrades)
        {
            upgradeDataDict.Add(ud.upgradeType, ud);
        }
    }
    public static event Action<Upgrade> OnPurchaseUpgrade;

    public void AttemptPurchaseUpgrade(Upgrade eUpgradeType)
    {
        OnPurchaseUpgrade?.Invoke(eUpgradeType);
    }
    public void ClearLevel()
    {
        OnClearLevel?.Invoke();
    }
}
