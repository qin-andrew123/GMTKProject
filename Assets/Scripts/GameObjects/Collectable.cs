using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 2.0f;
    [SerializeField] private GameObject floatingText;
    [SerializeField] private Color textColor;
    private int numRssDropped = 0;
    public int NumRssDropped
    {
        set { numRssDropped = value; }
    }
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
    private void Start()
    {
        collectableRB = GetComponent<Rigidbody2D>();
        bool ShouldMagnet = playerRef.GetComponent<Player>().DoResourcesMagnet;
        if(ShouldMagnet)
        {
            StartCoroutine(DelayTillCollect());
        }
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

            GlobalData.Instance.AdjustCurrency(numRssDropped);

            Destroy(gameObject);
        }
    }
}
