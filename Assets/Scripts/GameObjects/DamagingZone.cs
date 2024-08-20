using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagingZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip damageSFX;

    void Start()
    {
        playerAudioSource = GlobalData.Instance.playerReference.GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            Player player = collision.gameObject.GetComponent<Player>();

            if (!playerHealth || !player)
            {
                Debug.LogError("Error: Missing player on collided player object");
                return;
            }
            if (playerHealth.CanTakeDamage)
            {
                int modifyingDamage = damageAmount;
                if(player.CanHalveEnvDamage)
                {
                    float halvedDamage = modifyingDamage / 2f;
                    if ((halvedDamage <= 0.5))
                    {
                        modifyingDamage = 0;
                    }
                    else
                    {
                        modifyingDamage = (int)halvedDamage;
                    }
                }
                if (player.IsImmuneToEnvDamage)
                {
                    modifyingDamage = 0;
                }
                
                playerHealth.AdjustHealth(-modifyingDamage);

                // Play SFX
                playerAudioSource.PlayOneShot(damageSFX, 0.2f);
            }
        }
    }
}
