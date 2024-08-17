using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderComponent : MonoBehaviour
{
    private float xWidth = 0f;
    private float yHeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            return;
        }
        xWidth = spriteRenderer.bounds.size.x;
        yHeight = spriteRenderer.bounds.size.y;

        Debug.Log("Width: " + xWidth + "\n Height: " + yHeight);
    }
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision && collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (!playerController)
            {
                return;
            }
            playerController.GetComponent<Rigidbody2D>().gravityScale = 0f;
            playerController.CanClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision && collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (!playerController)
            {
                return;
            }
            playerController.GetComponent<Rigidbody2D>().gravityScale = 1f;
            playerController.CanClimb = false;
        }
    }
}
