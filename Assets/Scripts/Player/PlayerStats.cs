using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Material
{
    Scales
}
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public float invulnerabilityTime;
    // Materials should be saved across levels and can be used every upgrade station
    [Tooltip("This should be initialized first will all possible materials player can access")]
    public List<Material> playerMaterials;
    public List<Upgrade> playerUpgrades;
    public List<string> Explanation;
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

