using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeIconComponent : MonoBehaviour
{
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Image costImage;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    private Upgrade upgradeType;
    public void InitializeComponents(Sprite inputImage, Sprite costInputImage, string description, string name, string cost, Upgrade eUpgradeType)
    {
        upgradeImage.sprite = inputImage;
        costImage.sprite = costInputImage;
        upgradeDescription.text = description;
        upgradeName.text = name;
        upgradeCost.text = cost;
        upgradeType = eUpgradeType;
    }

    public void AttemptUpgrade()
    {
        GlobalData.Instance.AttemptPurchaseUpgrade(upgradeType);
        if (GlobalData.Instance.purchasedUpgrades.Contains(upgradeType))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
