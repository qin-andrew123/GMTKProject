using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;
    [SerializeField] private CompositeCollider2D[] allCameraBoundaries;
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;
    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    private float normalYPanAmount;
    private Vector2 startingTrackedObjectOffset;
    private bool bOverrideLookatLerping = false;
    public bool OverrideLookatLerping
    {
        get { return bOverrideLookatLerping; }
        set { bOverrideLookatLerping = value; }
    }

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip cavernAudio;
    [SerializeField] private AudioClip gemAudio;
    [SerializeField] private AudioClip waterAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        currentCamera = allVirtualCameras[0];
        framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        for (int i = 1; i < allVirtualCameras.Length; i++)
        {
            allVirtualCameras[i].enabled = false;
        }

        normalYPanAmount = framingTransposer.m_YDamping;

        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;

        // Start ambient audio
        playerAudioSource = GetComponent<AudioSource>();
        playerAudioSource.clip = cavernAudio;
        playerAudioSource.Play();
    }

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection ePanDirection, bool bPanToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(IEPanCameraOnContact(panDistance, panTime, ePanDirection, bPanToStartingPos));
    }

    public void UpdateMainCamera(int cameraIndex)
    {
        if (allVirtualCameras.Length <= cameraIndex)
        {
            Debug.LogError("Error: Invalid camera index set. Make sure that you have the proper cameras set up in camera manager");
            return;
        }
        CinemachineVirtualCamera oldCamera = currentCamera;
        oldCamera.enabled = false;
        currentCamera = allVirtualCameras[cameraIndex];
        currentCamera.enabled = true;
        UpdateConfiningShape(GlobalData.Instance.playerReference.transform.position);

        // Update ambient audio
        if (cameraIndex == 0)   // in main pit
        {
            playerAudioSource.clip = cavernAudio;
            playerAudioSource.Play();
        }
        else if (cameraIndex == 1)  // in level
        {
            // TO DO: add if statements depending on gem or water
            playerAudioSource.clip = gemAudio;
            playerAudioSource.Play();
        }
    }

    private IEnumerator IEPanCameraOnContact(float panDistance, float panTime, PanDirection ePanDirection, bool bPanToStartingPos)
    {
        Vector2 endpos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!bPanToStartingPos)
        {
            switch (ePanDirection)
            {
                case PanDirection.Up:
                    endpos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endpos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endpos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endpos = Vector2.right;
                    break;
                default:
                    break;
            }

            endpos *= panDistance;
            startingPos = startingTrackedObjectOffset;
            endpos += startingPos;
        }
        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endpos = startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 panLerp = Vector3.Lerp(startingPos, endpos, panTime / elapsedTime);
            framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    public void UpdateConfiningShape(Vector3 position)
    {
        CompositeCollider2D bestCollider = null;
        float optDistance = float.MaxValue;
        foreach (var iter in allCameraBoundaries)
        {
            if (iter == null) return;
            float distance = Vector3.Distance(iter.transform.position, position);
             if (distance < optDistance)
            {
                bestCollider = iter;
                optDistance = distance;
            }
        }

        if (!bestCollider)
        {
            Debug.LogWarning("Warning: best collider in UpdateConfiningShape returned a null composite, this is bad");
            return;
        }
        currentCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = bestCollider;
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normalYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYPanTime));
            yield return null;
        }
        IsLerpingYDamping = false;
    }

    public bool SwapCamera(CinemachineVirtualCamera leftCamera, CinemachineVirtualCamera rightCamera, Vector2 triggerExitDirection)
    {
        if (currentCamera == leftCamera && triggerExitDirection.x > 0)
        {
            rightCamera.enabled = true;
            leftCamera.enabled = false;
            currentCamera = rightCamera;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            // Update ambient audio
            if (currentCamera == allVirtualCameras[0])   // in main pit
            {
                playerAudioSource.clip = cavernAudio;
                playerAudioSource.Play();
            }
            else if (currentCamera == allVirtualCameras[1])  // in level
            {
                // TO DO: add if statements depending on gem or water
                playerAudioSource.clip = waterAudio;
                playerAudioSource.Play();
            }
            
            CameraManager.Instance.UpdateConfiningShape(GlobalData.Instance.playerReference.transform.position);
            return true;
        }
        else if (currentCamera == rightCamera && triggerExitDirection.x < 0)
        {
            leftCamera.enabled = true;
            rightCamera.enabled = false;
            currentCamera = leftCamera;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            // Update ambient audio
            if (currentCamera == allVirtualCameras[0])   // in main pit
            {
                playerAudioSource.clip = cavernAudio;
                playerAudioSource.Play();
            }
            else if (currentCamera == allVirtualCameras[1])  // in level
            {
                // TO DO: add if statements depending on gem or water
                playerAudioSource.clip = waterAudio;
                playerAudioSource.Play();
            }
            
            CameraManager.Instance.UpdateConfiningShape(GlobalData.Instance.playerReference.transform.position);
            return true;
        }
        return false;
    }
}
