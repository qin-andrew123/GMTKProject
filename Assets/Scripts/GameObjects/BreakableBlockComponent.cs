using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockComponent : MonoBehaviour
{
    [Tooltip("This value is compared against the player's tool. if this number is lower, this block will break")]
    [Range(1, 3)]
    [SerializeField] private int blockHardness = 0;
    public void AttemptBreakBlock(int destructionLevel)
    {
        if(blockHardness <= destructionLevel)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject && collision.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
    }
}
