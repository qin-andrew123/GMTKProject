using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LadderComponent : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision && collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if(playerController != null)
            {
                playerController.CanClimb = true;
                playerController.Grounded = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision && collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.CanClimb = false;
            }
        }

    }
}
