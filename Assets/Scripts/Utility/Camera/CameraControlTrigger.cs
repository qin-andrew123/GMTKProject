using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInpsectorObjects;

    private Collider2D triggerCollider2D;

    private void Start()
    {
        triggerCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (customInpsectorObjects.panCameraOnContact)
            {
                CameraManager.Instance.OverrideLookatLerping = true;
                CameraManager.Instance.PanCameraOnContact(
                    customInpsectorObjects.panDistance,
                    customInpsectorObjects.panTime,
                    customInpsectorObjects.ePanDirection,
                    false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (customInpsectorObjects.panCameraOnContact)
            {
                CameraManager.Instance.OverrideLookatLerping = false;
                CameraManager.Instance.PanCameraOnContact(
                    customInpsectorObjects.panDistance,
                    customInpsectorObjects.panTime,
                    customInpsectorObjects.ePanDirection,
                    true);
            }
            else if (customInpsectorObjects.swapCameras && customInpsectorObjects.cameraOnLeft && customInpsectorObjects.cameraOnRight)
            {
                Vector2 exitDirection = (collision.transform.position - triggerCollider2D.bounds.center).normalized;
                CameraManager.Instance.SwapCamera(customInpsectorObjects.cameraOnLeft, customInpsectorObjects.cameraOnRight, exitDirection);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool panCameraOnContact = false;
    public bool swapCameras = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection ePanDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (cameraControlTrigger.customInpsectorObjects.swapCameras)
        {
            cameraControlTrigger.customInpsectorObjects.cameraOnLeft = EditorGUILayout.ObjectField(
                "Camera on Left",
                cameraControlTrigger.customInpsectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            cameraControlTrigger.customInpsectorObjects.cameraOnRight = EditorGUILayout.ObjectField(
                "Camera on Right",
                cameraControlTrigger.customInpsectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if (cameraControlTrigger.customInpsectorObjects.panCameraOnContact)
        {
            cameraControlTrigger.customInpsectorObjects.ePanDirection = (PanDirection)EditorGUILayout.EnumPopup(
                    "Camera Pan Direction",
                    cameraControlTrigger.customInpsectorObjects.ePanDirection);
            cameraControlTrigger.customInpsectorObjects.panDistance = EditorGUILayout.FloatField(
                    "Camera Pan Distance",
                    cameraControlTrigger.customInpsectorObjects.panDistance);
            cameraControlTrigger.customInpsectorObjects.panTime = EditorGUILayout.FloatField(
                    "Camera Pan Time",
                    cameraControlTrigger.customInpsectorObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }

}