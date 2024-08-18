using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineComponent : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float cameraOffsetAmount = 1.0f;
    [SerializeField] private float lerpTime = 1.0f;
    private Vector3 currentLookDir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if (!_virtualCamera)
        {
            Debug.LogError("Warning cinemachine has no virtual camera");
            return;
        }

        PlayerController.OnHorizontalChangeDirection += AdjustLookatPoint;
    }

    private void OnDestroy()
    {
        PlayerController.OnHorizontalChangeDirection -= AdjustLookatPoint;
    }
    private void AdjustLookatPoint(float horizontalValue)
    {
        CinemachineFramingTransposer transposer = _virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        if ((!transposer))
        {
            Debug.LogError("Error: Framing Transposer is not set for cinemachine camera");
            return;
        }
        if (currentLookDir.x != horizontalValue)
        {
            currentLookDir = transposer.m_TrackedObjectOffset;
            Vector3 newLookat = Vector3.zero;
            if (horizontalValue < 0)
            {
                newLookat = new Vector3(-cameraOffsetAmount, 0);
            }
            else if (horizontalValue > 0)
            {
                newLookat = new Vector3(cameraOffsetAmount, 0);
            }
            StartCoroutine(LerpLookAt(currentLookDir, newLookat));
        }
    }

    IEnumerator LerpLookAt(Vector3 startLocation, Vector3 endLocation)
    {
        float elapsedTime = 0.0f;
        CinemachineFramingTransposer transposer = _virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        if ((!transposer))
        {
            Debug.LogError("Error: Framing Transposer is not set for cinemachine camera");
            yield return null;
        }
        Vector3 location = Vector3.zero;
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            location = Vector3.Lerp(startLocation, endLocation, (elapsedTime / lerpTime));
            transposer.m_TrackedObjectOffset = location;

            yield return null;
        }
    }
}
