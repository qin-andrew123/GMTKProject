using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockComponent : MonoBehaviour
{
    [Tooltip("This value is compared against the player's tool. if this number is lower, this block will break")]
    [Range(1, 3)]
    [SerializeField] private int blockHardness = 0;
    [SerializeField] private AudioClip[] destroyedSFXs;
    public void AttemptBreakBlock(int destructionLevel)
    {
        if (blockHardness <= destructionLevel)
        {
            GlobalData.Instance.playerReference.GetComponent<AudioSource>().PlayOneShot(destroyedSFXs[blockHardness]);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
    }
}
