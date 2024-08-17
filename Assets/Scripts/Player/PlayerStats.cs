using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]

public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public int maxDamage;
    public float maxFuel;
    // Currency should be saved across levels and can be used every upgrade station
    public int playerCurrency;
}
