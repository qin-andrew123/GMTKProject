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
}
