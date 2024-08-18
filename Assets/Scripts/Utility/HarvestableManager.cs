using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableManager : MonoBehaviour
{
    [SerializeField] private bool bIsResourceNodesWeighted = false;
    [Tooltip("These are the harvest nodes that you want to manage. If Weighted, Make sure that resource nodes are placed heaviest weight first, lightest last")]
    [SerializeField] private List<GameObject> ResourceNodes = new List<GameObject>();
    [Tooltip("These are the weights attached to each resource node (Make sure that resource nodes are placed heaviest weight first, lightest last)")]
    [SerializeField] private List<int> ResourceNodeWeights = new List<int>();
    [SerializeField] private int numNodesToActivate = 1;
    [SerializeField] private float respawnTime = 1.0f;
    private int totalWeight = 0;
   
    // indices of active nodes and inactiveNodes
    private List<int> activeNodes = new List<int>();
    private List<int> inactiveNodes = new List<int>();
    private Dictionary<GameObject, int> nodeActiveIndex = new Dictionary<GameObject, int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(var node in ResourceNodes)
        {
            node.SetActive(false);
        }
        Harvestable.OnHarvest += TurnOffNode;
        ChooseNodesToActivate();

        for(int i = 0; i < activeNodes.Count; ++i)
        {
            nodeActiveIndex.Add(ResourceNodes[activeNodes[i]], activeNodes[i]);
        }
    }

    private void ChooseNodesToActivate()
    {
        if(bIsResourceNodesWeighted)
        {
            foreach(int i in ResourceNodeWeights)
            {
                totalWeight += i;
            }

            for(int i = 0; i < numNodesToActivate; ++i)
            {
                int randomWeight = UnityEngine.Random.Range(0, totalWeight);
                int currentWeight = 0;
                for(int j = 0; j < ResourceNodeWeights.Count; ++j)
                {
                    if(ResourceNodes[j].activeInHierarchy)
                    {
                        continue;
                    }
                    currentWeight += ResourceNodeWeights[j];
                    if(currentWeight <= randomWeight)
                    {
                        ResourceNodes[j].SetActive(true);
                        break;
                    }
                }
            }
        }

        // Update and separate which nodes are active vs which are inactive
        for(int i = 0; i < ResourceNodes.Count; ++i)
        {
            if(ResourceNodes[i].activeInHierarchy)
            {
                activeNodes.Add(i);
            }
            else
            {
                inactiveNodes.Add(i);
            }
        }

        // Add a remaining number of nodes based on how many nodes left we have to activate
        int currentActiveNodes = activeNodes.Count;
        while (currentActiveNodes < numNodesToActivate)
        {
            for (int i = 0; i < inactiveNodes.Count; ++i)
            {
                float random = UnityEngine.Random.Range(0f, 1f);
                if (random < 0.3f && !ResourceNodes[inactiveNodes[i]].activeInHierarchy)
                {
                    activeNodes.Add(inactiveNodes[i]);
                    ResourceNodes[inactiveNodes[i]].SetActive(true);
                    ++currentActiveNodes;
                    break;
                }
            }
        }
    }
    private void TurnOffNode(GameObject node)
    {
        if (nodeActiveIndex.ContainsKey(node))
        {
            Debug.Log("Harvest Manager Received Harvest Call");
            StartCoroutine(ResetTimer(node));
        }
    }

    IEnumerator ResetTimer(GameObject node)
    {
        node.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        node.SetActive(true);
    }

    private void OnDestroy()
    {
        Harvestable.OnHarvest -= TurnOffNode;
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
