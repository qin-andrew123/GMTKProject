using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenComponent : MonoBehaviour
{
    [SerializeField] private Sprite[] potentialLoadingArt;
    private float loadingTime;
    public float LoadingTime
    {
        set { loadingTime = value; }
    }
    private void OnEnable()
    {
        ActivateLoadingScreen();
    }

    private void ActivateLoadingScreen()
    {
        Sprite imageToPick = potentialLoadingArt[UnityEngine.Random.Range(0, potentialLoadingArt.Length)];
        GetComponent<Image>().sprite = imageToPick;
        gameObject.SetActive(true);
        StartCoroutine(LoadingTimer());
    }

    private IEnumerator LoadingTimer()
    {
        yield return new WaitForSeconds(loadingTime);
        gameObject.SetActive(false);
    }
}
