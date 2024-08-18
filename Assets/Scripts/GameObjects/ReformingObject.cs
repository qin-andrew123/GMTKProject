using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReformingObject : MonoBehaviour
{
    [Tooltip("Tells the obstacleManager if this is respawnable")]
    [SerializeField] protected bool bDoesRespawn = false;
    [Tooltip("Tells obstacleManager how long until it respawns")]
    [SerializeField] protected float respawnTime = 1.0f;
    [Tooltip("How long will you allow the player to remain on this platform")]
    [SerializeField] protected float timeTillBreak = 1.0f;

    protected ReformingObstacleManager parentManager = null;
    protected int index = 0;
    public ReformingObstacleManager ParentManager
    {
        get { return parentManager; }
        set { parentManager = value; }
    }

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public float RespawnTime
    {
        get { return respawnTime; }
    }
}
