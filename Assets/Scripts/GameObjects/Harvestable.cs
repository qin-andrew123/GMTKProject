using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField] private float distForHarvest = 3.0f;
    [SerializeField] private int numResourcesDropped = 1;
    [SerializeField] private CurrencyType eMaterialType;
    [SerializeField] private GameObject resourceSprites;
    [SerializeField] private TextMeshPro collectableDescription;

    [Header("Loot Crates")]
    [Tooltip("This is for end of dungeon reward")]
    [SerializeField] private bool bIsLootCrate = false;
    [Tooltip("Modifier for how much bonus loot to drop (if upgraded)")]
    [Range(3, 8)]
    [SerializeField] private int lootModifier;
    public int NumResourcesDropped
    {
        get { return numResourcesDropped; }
    }

    public CurrencyType MaterialType
    {
        get { return eMaterialType; }
    }
    public static event Action<GameObject> OnHarvest;

    private void Start()
    {
        // We don't care about showing this if it's a loot crate since it'll look cool
        if(!bIsLootCrate)
        {
            if (!collectableDescription)
            {
                Debug.LogWarning("Uh oh, a harvestable doesn't have it's description text mesh");
                return;
            }
            string descText = "x" + numResourcesDropped;
            collectableDescription.text = descText;
        }
    }
    private void OnEnable()
    {
        PlayerController.PlayerAttemptHarvest += QueryIsHarvestable;
    }
    private void OnDisable()
    {
        PlayerController.PlayerAttemptHarvest -= QueryIsHarvestable;
    }
    private void QueryIsHarvestable(GameObject player)
    {
        if(!player)
        {
            Debug.Log("Player is null uhoh");
        }
        if(Vector2.Distance(gameObject.transform.position, player.transform.position) <= distForHarvest)
        {
            if(player.GetComponent<Player>().DoGetBonusReward && bIsLootCrate)
            {
                int modifier = UnityEngine.Random.Range(2, lootModifier);
                numResourcesDropped *= modifier;
            }

            for(int i  = 0; i < numResourcesDropped; ++i)
            {
                float angle = UnityEngine.Random.Range(-75.0f, 75.0f) * Mathf.Deg2Rad;
                float xValue = Vector2.up.x * Mathf.Cos(angle) - Vector2.up.y * Mathf.Sin(angle);
                float yValue = Vector2.up.x * Mathf.Sin(angle) + Vector2.up.y * Mathf.Cos(angle);

                Vector2 shotDirection = new Vector2(xValue, yValue);
                GameObject miniRss = Instantiate(resourceSprites);
                miniRss.transform.position = gameObject.transform.position;
                miniRss.GetComponent<Rigidbody2D>().velocity = shotDirection * 5;
                miniRss.GetComponent<Collectable>().PlayerRef = player;
                string displayString = "+ " + eMaterialType;
                miniRss.GetComponent<Collectable>().FloatingString = displayString;
                miniRss.GetComponent<Collectable>().NumRssDropped = 1;
            }
            OnHarvest?.Invoke(gameObject);

            if (bIsLootCrate)
            {
                GlobalData.Instance.ClearLevel();
                gameObject.SetActive(false);
            }
        }
    }
}
