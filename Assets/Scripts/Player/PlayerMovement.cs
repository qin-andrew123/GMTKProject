using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 10.0f;
    [SerializeField] private float jumpUpSpeed = 2.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D playerRB;
    private bool bCanJump = true;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        if (!playerRB)
        {
            Debug.LogWarning("Warning: Rigidbody is not set");
            return;
        }
    }

    private void Update()
    {
        float xAxisValue = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(xAxisValue, 0f);
        moveDirection.Normalize();
    }
    private void FixedUpdate()
    {
        if (!playerRB)
        {
            Debug.LogWarning("Warning: Rigidbody is not set");
            return;
        }

        if(bCanJump && Input.GetAxisRaw("Jump") != 0)
        {
            moveDirection += new Vector2(0, jumpUpSpeed);
            bCanJump = false;
        }
        playerRB.velocity += moveDirection;
        if(Mathf.Abs(playerRB.velocity.x) > maxMovementSpeed)
        {
            float maxValue = playerRB.velocity.x < 0 ? -maxMovementSpeed : maxMovementSpeed;
            playerRB.velocity = new Vector2(maxValue, playerRB.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bCanJump = true;
        }
    }
}
