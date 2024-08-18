using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryComponent : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            CameraManager.Instance.UpdateConfiningShape(collision.gameObject.transform.position);
        }
    }
}
