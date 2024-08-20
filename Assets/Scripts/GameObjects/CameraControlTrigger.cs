using Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects cameraControlOptions;
    private Collider2D triggerCollider2D;
    private ZoneSwapperComponent zoneSwapperComponent;
    private bool bDoesHaveParent = false;
    private void Start()
    {
        zoneSwapperComponent = GetComponentInParent<ZoneSwapperComponent>();
        if (zoneSwapperComponent)
        {
            bDoesHaveParent = true;
        }
        triggerCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cameraControlOptions.panCameraOnContact)
            {
                CameraManager.Instance.OverrideLookatLerping = true;
                CameraManager.Instance.PanCameraOnContact(
                    cameraControlOptions.panDistance,
                    cameraControlOptions.panTime,
                    cameraControlOptions.ePanDirection,
                    false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cameraControlOptions.panCameraOnContact)
            {
                CameraManager.Instance.OverrideLookatLerping = false;
                CameraManager.Instance.PanCameraOnContact(
                    cameraControlOptions.panDistance,
                    cameraControlOptions.panTime,
                    cameraControlOptions.ePanDirection,
                    true);
            }
            else if (cameraControlOptions.swapCameras && cameraControlOptions.cameraOnLeft && cameraControlOptions.cameraOnRight)
            {
                Vector2 exitDirection = (collision.transform.position - triggerCollider2D.bounds.center).normalized;
                if (bDoesHaveParent)
                {
                    zoneSwapperComponent.TriggerActivated();
                }
                CameraManager.Instance.SwapCamera(cameraControlOptions.cameraOnLeft, cameraControlOptions.cameraOnRight, exitDirection);
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
#if UNITY_EDITOR 
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

        if (cameraControlTrigger.cameraControlOptions.swapCameras)
        {
            cameraControlTrigger.cameraControlOptions.cameraOnLeft = EditorGUILayout.ObjectField(
                "Camera on Left",
                cameraControlTrigger.cameraControlOptions.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            cameraControlTrigger.cameraControlOptions.cameraOnRight = EditorGUILayout.ObjectField(
                "Camera on Right",
                cameraControlTrigger.cameraControlOptions.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if (cameraControlTrigger.cameraControlOptions.panCameraOnContact)
        {
            cameraControlTrigger.cameraControlOptions.ePanDirection = (PanDirection)EditorGUILayout.EnumPopup(
                    "Camera Pan Direction",
                    cameraControlTrigger.cameraControlOptions.ePanDirection);
            cameraControlTrigger.cameraControlOptions.panDistance = EditorGUILayout.FloatField(
                    "Camera Pan Distance",
                    cameraControlTrigger.cameraControlOptions.panDistance);
            cameraControlTrigger.cameraControlOptions.panTime = EditorGUILayout.FloatField(
                    "Camera Pan Time",
                    cameraControlTrigger.cameraControlOptions.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}
#endif