using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private float verticalOffset;
    [SerializeField] private float horizontalOffset;
    [SerializeField] private TextMeshPro displayTextName;
    [SerializeField] private string DisplayText;
    private void Start()
    {
        Vector3 offset = new Vector2(horizontalOffset, verticalOffset);
        displayTextName.transform.localPosition += offset;
        displayTextName.text = DisplayText;
    }
    private void OnEnable()
    {
        displayTextName.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            displayTextName.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            displayTextName.gameObject.SetActive(false);
        }
    }
}
