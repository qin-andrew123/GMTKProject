using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CurrencyType
{
    Scales
}
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public float invulnerabilityTime;
    public int numCurrency = 0;
    public CurrencyType currencyType;
    [Tooltip("Order these in the order that we expect people to get these upgrades.")]
    public List<UpgradeData> playerUpgrades = new List<UpgradeData>();
}

public enum Upgrade
{
    SpringSpikes, //icejump
    RustySpring, //doublejump
    SwiftWings, //dash
    SturdyBoots, //half lava
    ObsidianBoots, //immune lava
    CharmedGrieves, //heal in lava
    BladedTool, //break vines
    FirmGrip, //break fragile ice
    BladeSpikes, //break ice
    Feather, //platforms dont fall
    FragileCap, //ignore dmg once
    BronzeHelmet, // ignore dmg twice
    GuildCoin, //cheaper upgrades
    LuckyClove, //double reward chance
    Magnet, //scales magnetize
    CharmOfBravery //+10 health
}

