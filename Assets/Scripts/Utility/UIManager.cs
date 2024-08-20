using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject currencyCounterUI;
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private Slider healthSliderUI;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        GlobalData.OnCurrencyAmountChange += AdjustCurrencyCounter;
        SpawnPortalComponent.OnTravellingToSpawn += ActivateLoadingScreen;
        PlayerHealth.OnPlayerHealthChanged += AdjustHealthInformation;
        AdjustCurrencyCounter();
        healthSliderUI.maxValue = GlobalData.Instance.GetInitMaxHealth();
        healthSliderUI.value = GlobalData.Instance.GetInitMaxHealth();
        textMeshProUGUI.text = (int)healthSliderUI.value + "/" + healthSliderUI.maxValue;
    }
    private void OnDestroy()
    {
        GlobalData.OnCurrencyAmountChange -= AdjustCurrencyCounter;
        SpawnPortalComponent.OnTravellingToSpawn -= ActivateLoadingScreen;
        PlayerHealth.OnPlayerHealthChanged -= AdjustHealthInformation;
    }
    private void AdjustHealthInformation(int amountHealth)
    {
        if (healthSliderUI.maxValue < amountHealth)
        {
            healthSliderUI.maxValue = amountHealth;
        }

        StartCoroutine(LerpHealthValue(amountHealth));
    }
    private IEnumerator LerpHealthValue(float amountHealth)
    {
        float elapsedTime = 0;
        float currHealth = healthSliderUI.value;
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedValue = Mathf.Lerp(currHealth, amountHealth, elapsedTime / lerpTime);
            healthSliderUI.value = lerpedValue;
            textMeshProUGUI.text = (int)healthSliderUI.value + "/" + healthSliderUI.maxValue;
            yield return null;
        }
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
