using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSwapperComponent : MonoBehaviour
{
    [SerializeField] private CameraControlTrigger trigger;
    [SerializeField] private EnterZoneComponent enterZoneComponent;
    private void Start()
    {
        GlobalData.OnClearLevel += DeactivateTrigger;
    }
    private void OnDestroy()
    {
        GlobalData.OnClearLevel -= DeactivateTrigger;
    }
    public void TriggerActivated()
    {
        enterZoneComponent.DenyPlayersReEntry();
    }

    private void DeactivateTrigger()
    {
        enterZoneComponent.AllowPlayersReEntry();
    }
}
