using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private float verticalOffset;
    [SerializeField] private float horizontalOffset;
    [SerializeField] private TextMeshPro displayTextName;
    [SerializeField] private string displayText;
    private void Start()
    {
        Vector3 offset = new Vector2(horizontalOffset, verticalOffset);
        displayTextName.text = displayText;
        displayTextName.transform.localPosition += offset;
    }
    private void OnEnable()
    {
        displayTextName.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!displayTextName.gameObject.activeInHierarchy)
            {
                displayTextName.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (displayTextName.gameObject.activeInHierarchy)
            {
                displayTextName.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        displayTextName.gameObject.SetActive(false);
    }
}
