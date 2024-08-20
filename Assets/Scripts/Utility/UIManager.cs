using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject currencyCounterUI;
    [SerializeField] private GameObject loadingScreenUI;
    // Start is called before the first frame update
    void Start()
    {
        GlobalData.OnCurrencyAmountChange += AdjustCurrencyCounter;
        SpawnPortalComponent.OnTravellingToSpawn += ActivateLoadingScreen;
        AdjustCurrencyCounter();
    }
    private void OnDestroy()
    {
        GlobalData.OnCurrencyAmountChange -= AdjustCurrencyCounter;
        SpawnPortalComponent.OnTravellingToSpawn -= ActivateLoadingScreen;
    }
    private void AdjustCurrencyCounter()
    {
        currencyCounterUI.GetComponent<TextMeshProUGUI>().text = "x " + GlobalData.Instance.GetCurrency().ToString();
    }
    private void ActivateLoadingScreen(float time)
    {
        loadingScreenUI.GetComponent<LoadingScreenComponent>().LoadingTime = time;
        loadingScreenUI.SetActive(true);
    }
}
