using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BoundsChecker : MonoBehaviour
{
    public enum eTypeBoundsCheck { center, inset, outset };
    [SerializeField] private eTypeBoundsCheck boundsCheckType;
    [SerializeField] private float boundsRadius;
    private float camWidth;
    private float camHeight;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        float checkRadius = 0;
        if (boundsCheckType == eTypeBoundsCheck.inset)
        {
            checkRadius = -boundsRadius;
        }
        else if (boundsCheckType == eTypeBoundsCheck.outset)
        {
            checkRadius = boundsRadius;
        }
        Vector2 transformPos = transform.position;
        if(transformPos.x > camWidth + checkRadius)
        {
            transformPos.x = camWidth + checkRadius;
        }
        if(transformPos.x < -camWidth - checkRadius)
        {
            transformPos.x = -camWidth - checkRadius;
        }
        if(transformPos.y  > camHeight + checkRadius)
        {
            transformPos.y = camHeight + checkRadius;
        }
        if(transformPos.y < -camHeight - checkRadius)
        {
            transformPos.y = -camHeight - checkRadius;
        }

        transform.position = transformPos;
    }
}
