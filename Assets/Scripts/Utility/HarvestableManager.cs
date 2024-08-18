using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableManager : MonoBehaviour
{
    [SerializeField] List<GameObject> ResourceNodes = new List<GameObject>();
    [SerializeField] private float respawnTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Harvestable.OnHarvest += TurnOffNode;
    }

    private void TurnOffNode(GameObject node)
    {
        if (ResourceNodes.Contains(node))
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
