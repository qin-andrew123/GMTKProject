using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Color textColor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float verticalFloat;
    [SerializeField] private float horizontalFloat;

    private TextMeshPro textMeshPro;
    private Vector2 translationVector;
    private float totalDistance;

    private string floatingString = "";
    public string FloatingString
    {
        set { floatingString = value; }
    }
    public Color TextColor
    {
        set { textColor = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        if(!textMeshPro)
        {
            Debug.LogError("Wrong TMP in Floating Text");
            return;
        }
        textMeshPro.color = textColor;
        textMeshPro.text = floatingString;
        translationVector = gameObject.transform.position + new Vector3(horizontalFloat, verticalFloat);
        totalDistance = Vector3.Distance(gameObject.transform.position, translationVector);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, translationVector);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, translationVector, moveSpeed * Time.deltaTime);
        textMeshPro.alpha = 255 * distance / totalDistance;
        if(distance < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
