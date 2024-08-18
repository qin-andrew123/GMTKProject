using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 2.0f;
    [SerializeField] private GameObject floatingText;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private Color textColor;

    private string floatingString;
    private GameObject playerRef;
    private Rigidbody2D collectableRB;
    private bool bCanMoveToPlayer = false;
    public GameObject PlayerRef
    {
        set { playerRef = value; }
    }
    public string FloatingString
    {
        set { floatingString = value; }
    }
    public void SetDescriptionText(string inputText)
    {
        descriptionText.text = inputText;
    }
    private void OnEnable()
    {
        collectableRB = GetComponent<Rigidbody2D>();
        StartCoroutine(DelayTillCollect());
    }
    // Update is called once per frame
    void Update()
    {
        if(bCanMoveToPlayer)
        {
            if(collectableRB.gravityScale != 0)
            {
                collectableRB.gravityScale = 0;
            }
            collectableRB.bodyType = RigidbodyType2D.Kinematic;
            collectableRB.transform.position = Vector3.Lerp(collectableRB.transform.position, playerRef.transform.position, lerpSpeed * Time.deltaTime);
        }
    }

    IEnumerator DelayTillCollect()
    {
        yield return new WaitForSeconds(1.0f);
        bCanMoveToPlayer = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            if(floatingText)
            {
                GameObject go = Instantiate(floatingText);
                go.transform.position = collision.gameObject.transform.position;
                go.GetComponent<FloatingText>().TextColor = textColor;
                go.GetComponent<FloatingText>().FloatingString = floatingString;
            }
            Destroy(gameObject);
        }
    }
}
