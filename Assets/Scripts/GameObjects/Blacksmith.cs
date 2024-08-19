using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private GameObject BlacksmithUI;
    [SerializeField] private GameObject slidingUIImage;
    [SerializeField] private GameObject uiUpgradePrefab;
    [SerializeField] private float activeDistance = 1.0f;
    [SerializeField] private float yPadding = 3.0f;
    public static event Action<bool> OnActivateBlacksmithUI;
    private float xStartingOffset = 8;
    private float yStartingOffset = 50f;
    
    private void Start()
    {
        BlacksmithUI.SetActive(false);
        InitializeUI();
        PlayerController.PlayerAttemptShop += QueryNearBlacksmith;  
    }

    private void InitializeUI()
    {
        float yStartingPosition = slidingUIImage.gameObject.GetComponent<RectTransform>().rect.height / 2 - yStartingOffset;
        float xStartingPosition = xStartingOffset;
        List<UpgradeData> upgrades = GlobalData.Instance.GetUpgradeData();
        for (int i = 0; i < upgrades.Count; ++i)
        {
            Upgrade upgradeType = upgrades[i].upgradeType;
            string upgradeName = upgrades[i].upgradeName;
            string upgradeDesc = upgrades[i].upgradeDescription;
            int upgradeCost = upgrades[i].currencyCost;
            Image upgradeImage = upgrades[i].upgradeImage;
            Image costImage = upgrades[i].costImage;

            GameObject uiPrefab = Instantiate(uiUpgradePrefab);
            uiPrefab.transform.SetParent(slidingUIImage.transform, false);
            
            uiPrefab.transform.localPosition = new Vector2(xStartingPosition, yStartingPosition);

            uiPrefab.GetComponent<UpgradeIconComponent>().InitializeComponents(upgradeImage, costImage, upgradeDesc, upgradeName, upgradeCost.ToString(),upgradeType);

            yStartingPosition -= (yStartingOffset + yPadding);
        }
    }
    private void OnDestroy()
    {
        PlayerController.PlayerAttemptShop -= QueryNearBlacksmith;
    }
    private void QueryNearBlacksmith(GameObject player)
    {
        Debug.Log("Running QNB");
        if (!player)
        {
            Debug.Log("Player is null uhoh");
            return;
        }
        if(Vector2.Distance(player.transform.position, transform.position) < activeDistance)
        {
            if(BlacksmithUI.activeInHierarchy)
            {
                BlacksmithUI.SetActive(false);
            }
            else
            {
                BlacksmithUI.SetActive(true);
            }
            OnActivateBlacksmithUI?.Invoke(BlacksmithUI.activeInHierarchy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            BlacksmithUI.SetActive(false);
            OnActivateBlacksmithUI?.Invoke(BlacksmithUI.activeInHierarchy);
        }
    }
}
