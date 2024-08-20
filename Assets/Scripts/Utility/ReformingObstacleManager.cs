using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReformingObstacleManager : MonoBehaviour
{
    [Tooltip("Contains all obstacles that can reform. Currently vines and reformingplatforms")]
    [SerializeField] private List<GameObject> reformingObstacles;
    private bool bReadyToRespawn = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < reformingObstacles.Count; ++i)
        {
            ReformingObject reformingObject = reformingObstacles[i].GetComponent<ReformingObject>();
            if (reformingObject)
            {
                reformingObject.ParentManager = this;
                reformingObject.Index = i;
            }
            else
            {
                Debug.Log("Error, there was a polymorphism error or something");
            }
        }
    }

    public void DeactivateReformingObject(ReformingObject input, int index, bool bDoesRespawn, float timeToBreak = 0)
    {
        if (reformingObstacles[index].GetComponent<ReformingObject>() != input)
        {
            Debug.Log("No Match between input and index");
            return;
        }

        if (timeToBreak == 0)
        {
            reformingObstacles[index].SetActive(false);
        }
        else
        {
            StartCoroutine(DestroyObject(timeToBreak, index));
        }
        if (bDoesRespawn && bReadyToRespawn)
        {
            StartCoroutine(RespawnObject(input.RespawnTime, index));
        }
    }
    private IEnumerator DestroyObject(float timeToBreak, int index)
    {
        yield return new WaitForSeconds(timeToBreak);
        reformingObstacles[index].SetActive(false);
        bReadyToRespawn = true;
    }
    private IEnumerator RespawnObject(float reformTime, int index)
    {
        yield return new WaitForSeconds(reformTime);
        reformingObstacles[index].SetActive(true);
        bReadyToRespawn = false;
    }
}
