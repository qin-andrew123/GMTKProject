using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceZoneComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            Player playerComponent = collision.gameObject.GetComponent<Player>();
            PlayerController controller = playerComponent.GetComponent<PlayerController>();
            if(!playerComponent && !controller)
            {
                Debug.LogWarning("Warning: " + collision.gameObject + " has entered an icezone without player/playercontroller components and is tagged as player");
                return;
            }

            if(!playerComponent.HasIceBoots)
            {
                controller.SlidingOnIce = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            Player playerComponent = collision.gameObject.GetComponent<Player>();
            PlayerController controller = playerComponent.GetComponent<PlayerController>();
            if (!playerComponent && !controller)
            {
                Debug.LogWarning("Warning: " + collision.gameObject + " has entered an icezone without player/playercontroller components and is tagged as player");
                return;
            }

            controller.SlidingOnIce = false;
        }
    }
}
