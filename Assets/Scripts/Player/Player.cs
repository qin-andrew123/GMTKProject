using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    private Dictionary<Material, int> materialCountDict = new Dictionary<Material, int>();
    private int currentHealth;
    private void InitializeStats()
    {
        if(!stats)
        {
            return;
        }

        foreach(var iter in stats.playerMaterials)
        {
            materialCountDict.Add(iter, 0);
        }
        currentHealth = stats.maxHealth;
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
        Debug.Log("Material Type: " + harvestable.MaterialType + " added " +  harvestable.NumResourcesDropped + " to player inventory");
        Debug.Log("Current amount for material (mat, amt): (" + harvestable.MaterialType + ", " + materialCountDict[harvestable.MaterialType] + ")");
    }

    private void OnDestroy()
    {
        Harvestable.OnHarvest -= OnHarvestData;
    }
}
