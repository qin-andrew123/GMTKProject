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
