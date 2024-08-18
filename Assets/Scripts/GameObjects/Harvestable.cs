using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField] private float distForHarvest = 3.0f;
    [SerializeField] private int numResourcesDropped = 1;
    [SerializeField] private Material eMaterialType;
    [SerializeField] private GameObject resourceSprites;
    public int NumResourcesDropped
    {
        get { return numResourcesDropped; }
    }

    public Material MaterialType
    {
        get { return eMaterialType; }
    }
    public static event Action<GameObject> OnHarvest;
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
            for(int i  = 0; i < numResourcesDropped; ++i)
            {
                float angle = UnityEngine.Random.Range(-75.0f, 75.0f) * Mathf.Deg2Rad;
                float xValue = Vector2.up.x * Mathf.Cos(angle) - Vector2.up.y * Mathf.Sin(angle);
                float yValue = Vector2.up.x * Mathf.Sin(angle) + Vector2.up.y * Mathf.Cos(angle);

                Vector2 shotDirection = new Vector2(xValue, yValue);
                GameObject miniRss = Instantiate(resourceSprites);
                miniRss.transform.position = gameObject.transform.position;
                miniRss.GetComponent<Rigidbody2D>().velocity += shotDirection * 3;
                miniRss.GetComponent<Collectable>().PlayerRef = player;
                string descText = "x" + numResourcesDropped;
                miniRss.GetComponent<Collectable>().SetDescriptionText(descText);
                string displayString = "+ " + eMaterialType;
                Debug.Log(displayString);
                miniRss.GetComponent<Collectable>().FloatingString = displayString;
            }
            OnHarvest?.Invoke(gameObject);
        }
    }
}
