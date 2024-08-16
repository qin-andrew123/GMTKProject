using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float destroyTime;
    private Vector2 moveDirection;
    public Vector2 MoveDirection 
    { 
        get { return moveDirection; } 
        set { moveDirection = value; } 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnEnable()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
