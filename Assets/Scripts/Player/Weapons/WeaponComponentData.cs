using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]

public class WeaponComponentData : ScriptableObject
{
    public int numProjectilesShot;
    public float fireCooldown;
    public GameObject ProjectilePrefab;
}
