using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData", order = 3)]
public class UpgradeData : ScriptableObject
{
    public Upgrade upgradeType;
    public string upgradeName;
    public string upgradeDescription;
    public int currencyCost;
    public Sprite upgradeImage;
    public Sprite costImage;
}
