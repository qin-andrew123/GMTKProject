using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject scalesCounter;
    private Dictionary<Material, int> materialCountDict = new Dictionary<Material, int>();
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
    private bool bCanDash = true; // Dash

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

    private bool bCanBeHealedByEnv = false; // Env dmg heals
    public bool CanBeHealedByEnv
    {
        get { return bCanBeHealedByEnv; }
        set { bCanBeHealedByEnv = value; }
    }
    private bool bCanDestroyHazards = false; // Break vines
    public bool CanBreakHurtBoxes
    {
        get { return bCanDestroyHazards; }
        set { bCanDestroyHazards = value; }
    }
    private int obstacleDestructionLevel = 0; // 0 == cannot break, 1 == vines, 2 == mid level, 3 >= all
    public int ObstacleDestructionLevel
    {
        get { return obstacleDestructionLevel; }
        set 
        {
            if (value > 1)
            {
                ++obstacleDestructionLevel;
            }
            else
            {
                obstacleDestructionLevel += value;
            }
        }
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
        set 
        {
            if(value > 1)
            {
                ++ObstacleImmunityLevel;
            }
            else
            {
                obstacleImmunityLevel += value;
            }
        }
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
        if(!stats)
        {
            return;
        }

        for(int i = 0; i < stats.playerMaterials.Count; ++i)
        {
            materialCountDict.Add(stats.playerMaterials[i], 0);
            // If we want more, then we will have to refactor this
            scalesCounter.GetComponentInChildren<TextMeshProUGUI>().SetText(stats.playerMaterials[i].ToString());
        }
    }
    void Start()
    {
        InitializeStats();
        Harvestable.OnHarvest += OnHarvestData;
    }

    private void OnHarvestData(GameObject node)
    {
        Harvestable harvestable = node.GetComponent<Harvestable>();
        if(!harvestable)
        {
            Debug.Log("On Harvest Data harvested a node with no harvestable script");
            return;
        }
        if(materialCountDict.ContainsKey(harvestable.MaterialType))
        {
            materialCountDict[harvestable.MaterialType] += harvestable.NumResourcesDropped;
        }
        else
        {
            materialCountDict[harvestable.MaterialType] = harvestable.NumResourcesDropped;
        }
        scalesCounter.GetComponentInChildren<TextMeshProUGUI>().SetText(stats.playerMaterials[0].ToString() + ": " + materialCountDict[harvestable.MaterialType]);
    }

    private void OnDestroy()
    {
        Harvestable.OnHarvest -= OnHarvestData;
    }
}
