using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Slider FuelUI;
    [SerializeField] private TextMeshProUGUI text;
    private int currentHealth;
    private int currentDamage;
    private float currentFuel;
    
    public void AdjustFuel(float fuel)
    {
        currentFuel += fuel;
    }
    private void InitializeStats()
    {
        if(!stats)
        {
            return;
        }
        currentHealth = stats.maxHealth;
        currentDamage = stats.maxDamage;
        currentFuel = stats.maxFuel;
    }
    void Start()
    {
        InitializeStats();

        if(!FuelUI)
        {
            return;
        }
        FuelUI.maxValue = currentFuel;
        FuelUI.value = currentFuel;
        text.SetText("Fuel: " + System.Math.Round((currentFuel / stats.maxFuel) * 100, 0) + "%");
    }

    public void UpdateFuelBar()
    {
        FuelUI.value = currentFuel;
        if(!text)
        {
            Debug.Log("wrong TMP");
        }
        text.SetText("Fuel: " + System.Math.Round((currentFuel / stats.maxFuel)* 100 ,0) + "%");
    }
}
