using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatformComponent : ReformingObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Vector2 directionOfContact = collision.GetContact(0).normal;
            Debug.Log(directionOfContact);
            if (player && directionOfContact == Vector2.down)
            {
                if (!player.DoesHaveFeather)
                {
                    parentManager.DeactivateReformingObject(this, index, bDoesRespawn, timeTillBreak);
                }
            }
        }
    }
}
