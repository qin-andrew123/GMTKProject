using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableManager : MonoBehaviour
{
    [Tooltip("These are the harvest nodes that you want to manage. If Weighted, Make sure that resource nodes are placed heaviest weight first, lightest last")]
    [SerializeField] private List<GameObject> ResourceNodes = new List<GameObject>();
    [SerializeField] private int numNodesToActivate = 1;
    private Dictionary<GameObject, int> nodeIndex = new Dictionary<GameObject, int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var node in ResourceNodes)
        {
            node.SetActive(true);
        }
        Harvestable.OnHarvest += TurnOffNode;
        SpawnPortalComponent.OnTravellingToSpawn += ChooseNodesToActivate;
        PlayerHealth.OnPlayerDie += ChooseNodesToActivate;
        ChooseNodesToActivate();
    }

    private void ChooseNodesToActivate(float time)
    {
        ChooseNodesToActivate();
    }
    private void ChooseNodesToActivate()
    {
        // We want to reset on respawn / return to base
        if (nodeIndex.Count > 0)
        {
            nodeIndex.Clear();
        }

        // Add spawned nodes 
        for (int i = 0; i < ResourceNodes.Count; ++i)
        {
            if (!ResourceNodes[i].activeInHierarchy)
            {
                if (ResourceNodes[i].GetComponent<Harvestable>())
                {
                    if (ResourceNodes[i].GetComponent<Harvestable>().CanRespawn)
                    {
                        ResourceNodes[i].SetActive(true);
                        nodeIndex.Add(ResourceNodes[i], i);
                    }
                }
            }
            else
            {
                if (!nodeIndex.ContainsKey(ResourceNodes[i]))
                {
                    nodeIndex.Add(ResourceNodes[i], i);
                }
            }
        }
    }


    private void TurnOffNode(GameObject node)
    {
        if (nodeIndex.ContainsKey(node))
        {
            Debug.Log("Harvest Manager Received Harvest Call. Turning off:" + node.name);
            node.SetActive(false);
        }
    }


    private void OnDestroy()
    {
        Harvestable.OnHarvest -= TurnOffNode;
        SpawnPortalComponent.OnTravellingToSpawn -= ChooseNodesToActivate;
        PlayerHealth.OnPlayerDie -= ChooseNodesToActivate;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (GameObject node in ResourceNodes)
        {
            Harvestable harvestable = node.GetComponent<Harvestable>();
            if (!harvestable)
            {
                Debug.LogWarning("Error: " + node.name + " does not have a harvestable script attached. make sure this is something you want in this list");
            }
        }
    }
#endif
}
