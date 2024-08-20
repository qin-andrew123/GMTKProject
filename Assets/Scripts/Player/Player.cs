using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject scalesCounter;
    public Dictionary<Upgrade, string> upgradeName = new Dictionary<Upgrade, string>();
    public Dictionary<Upgrade, string> upgradeExplanation = new Dictionary<Upgrade, string>();
    public static event Action<Upgrade, int> OnUpgradePurchased;
    private bool bOverrideIce = false; // Ice Boots
    public bool HasIceBoots
    {
        get { return bOverrideIce; }
        set { bOverrideIce = value; }
    }
    private bool bCanDoubleJump = false; // Spring
    public bool CanDoubleJump
    {
        get { return bCanDoubleJump; }
        set { bCanDoubleJump = value; }
    }
    private bool bCanDash = false; // Dash

    public bool CanDash
    {
        get { return bCanDash; }
        set { bCanDash = value; }
    }
    private bool bCanHalveEnvDamage = false; // Halve lava damage
    public bool CanHalveEnvDamage
    {
        get { return bCanHalveEnvDamage; }
        set { bCanHalveEnvDamage = value; }
    }
    private bool bIsImmuneToEnvDamage = false; // Env dmg immunity
    public bool IsImmuneToEnvDamage
    {
        get { return bIsImmuneToEnvDamage; }
        set { bIsImmuneToEnvDamage = value; }
    }
    private bool bDoesHealInLava = false;
    public bool DoesHealInLava
    {
        get { return bDoesHealInLava; }
        set { bDoesHealInLava = value; }
    }
    private int obstacleDestructionLevel = 0; // 0 == cannot break, 1 == vines, 2 == mid level, 3 >= all
    public int ObstacleDestructionLevel
    {
        get { return obstacleDestructionLevel; }
    }
    private bool bCanStopLedgeBreaking = false; // feather --> stops temp ledges from breaking
    public bool DoesHaveFeather
    {
        get { return bCanStopLedgeBreaking; }
        set { bCanStopLedgeBreaking = value; }
    }
    private int obstacleImmunityLevel = 0; // 0 = cannot dodge obst damage, 1 = once, 2 >= 2 times
    public int ObstacleImmunityLevel
    {
        get { return obstacleImmunityLevel; }
    }

    private bool bAreUpgradesCheaper = false; // CheaperUpgrades
    public bool AreUpgradesCheaper
    {
        get { return bAreUpgradesCheaper; }
        set { bAreUpgradesCheaper = value; }
    }
    private bool bDoesGetBonusReward = false;

    public bool DoGetBonusReward
    {
        get { return bDoesGetBonusReward; }
        set { bDoesGetBonusReward = value; }
    }
    private bool bDoResourcesMagnet = false;

    public bool DoResourcesMagnet
    {
        get { return bDoResourcesMagnet; }
        set { bDoResourcesMagnet = value; }
    }
    private void InitializeStats()
    {
        // If we want more, then we will have to refactor this
        scalesCounter.GetComponentInChildren<TextMeshProUGUI>().SetText(GlobalData.Instance.GetCurrency().ToString());
    }
    void Start()
    {
        InitializeStats();
        GlobalData.OnPurchaseUpgrade += EvaluateUpgrade;
        GlobalData.OnCurrencyAmountChange += OnCurrencyChanged;
    }

    private void EvaluateUpgrade(Upgrade eUpgradeType)
    {
        UpgradeData upgrade;
        GlobalData.Instance.upgradeDataDict.TryGetValue(eUpgradeType, out upgrade);
        if (!upgrade)
        {
            Debug.LogError("Error: Global Data singleton could not find this upgrade type");
        }

        int currentCurrency = GlobalData.Instance.GetCurrency();
        int upgradeCost = upgrade.currencyCost;
        upgradeCost = bAreUpgradesCheaper ? upgradeCost / 2 : upgradeCost;

        bool bCanPurchase = currentCurrency >= upgradeCost;
        if (bCanPurchase)
        {
            switch (eUpgradeType)
            {
                case Upgrade.SpringSpikes: //icejump
                    bOverrideIce = true;
                    break;
                case Upgrade.RustySpring: //doublejump
                    bCanDoubleJump = true;
                    OnUpgradePurchased(eUpgradeType, 1);
                    break;
                case Upgrade.SwiftWings: //dash
                    bCanDash = true;
                    break;
                case Upgrade.SturdyBoots: //half lava
                    bCanHalveEnvDamage = true;
                    break;
                case Upgrade.ObsidianBoots: //immune lava
                    bIsImmuneToEnvDamage = true;
                    break;
                case Upgrade.CharmedGrieves: //heal in lava
                    bDoesHealInLava = true;
                    break;
                case Upgrade.BladedTool: //break vines
                    obstacleDestructionLevel += 1;
                    break;
                case Upgrade.FirmGrip: //break fragile ice
                    obstacleDestructionLevel += 2;
                    break;
                case Upgrade.BladeSpikes: //break ice
                    obstacleDestructionLevel += 3;
                    break;
                case Upgrade.Feather: //platforms dont fall
                    bCanStopLedgeBreaking = true;
                    break;
                case Upgrade.FragileCap: //ignore dmg once
                    obstacleImmunityLevel += 1;
                    break;
                case Upgrade.BronzeHelmet: // ignore dmg twice
                    obstacleImmunityLevel += 2;
                    break;
                case Upgrade.GuildCoin: //cheaper upgrades
                    bAreUpgradesCheaper = true;
                    break;
                case Upgrade.LuckyClove: //double reward chance
                    bDoesGetBonusReward = true;
                    break;
                case Upgrade.Magnet: //scales magnetize
                    bDoResourcesMagnet = true;
                    break;
                case Upgrade.CharmOfBravery://+10 health
                    OnUpgradePurchased?.Invoke(eUpgradeType, 10);
                    break;
                default:
                    break;
            }
            if (obstacleImmunityLevel > 2)
            {
                obstacleImmunityLevel = 2;
            }
            if (obstacleImmunityLevel != 0 && eUpgradeType == Upgrade.FragileCap || eUpgradeType == Upgrade.BronzeHelmet)
            {
                OnUpgradePurchased?.Invoke(eUpgradeType, obstacleImmunityLevel);
            }

            GlobalData.Instance.purchasedUpgrades.Add(eUpgradeType);
            GlobalData.Instance.AdjustCurrency(currentCurrency - upgradeCost);
        }

    }
    private void OnCurrencyChanged()
    {
        scalesCounter.GetComponentInChildren<TextMeshProUGUI>().SetText(GlobalData.Instance.GetCurrencyType().ToString() + ": " + GlobalData.Instance.GetCurrency());
    }


    private void OnDestroy()
    {
        GlobalData.OnPurchaseUpgrade -= EvaluateUpgrade;
        GlobalData.OnCurrencyAmountChange -= OnCurrencyChanged;
    }
}
