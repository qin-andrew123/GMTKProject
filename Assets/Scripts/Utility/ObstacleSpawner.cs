using System.Collections;
using UnityEngine;

public enum TriggerDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public class ObstacleSpawner : MonoBehaviour
{
    [Tooltip("Enable this bool to make it so that this will trigger if a player walks over it instead of consistently")]
    [SerializeField] private bool bIsTrap;
    [Tooltip("How far away do you want this trap to trigger?")]
    [SerializeField] private float triggerDistance;
    [Tooltip("What layer do you want to detect?")]
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private TriggerDirection eTrapDirection;
    [SerializeField] private float spawnerRefreshRate = 1.0f;
    [Tooltip("This is the object that the trap will shoot out")]
    [SerializeField] private GameObject trapObject;
    private Vector2 trapTriggerDirection;
    private bool bCanLaunchTrap = true;
    private bool bHasLaunchedTrap = false;
    // Start is called before the first frame update
    void Start()
    {
        switch (eTrapDirection)
        {
            case TriggerDirection.UP:
                trapTriggerDirection = Vector2.up;
                break;
            case TriggerDirection.DOWN:
                trapTriggerDirection = Vector2.down;
                break;
            case TriggerDirection.LEFT:
                trapTriggerDirection = Vector2.left;
                break;
            case TriggerDirection.RIGHT:
                trapTriggerDirection = Vector2.right;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsTrap)
        {
            if (bHasLaunchedTrap)
            {
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, trapTriggerDirection, triggerDistance, detectionLayer);
            if (hit)
            {
                GameObject trap = Instantiate(trapObject, transform.position, Quaternion.identity);
                ObstacleComponent obstacleComponent = trap.GetComponent<ObstacleComponent>();
                if (!obstacleComponent)
                {
                    Debug.LogError("Error: There is no obstacle component attached to the trap object prefab");
                    return;
                }
                obstacleComponent.SetObjectVelocity(trapTriggerDirection);
                bHasLaunchedTrap = true;
            }
        }
        else
        {
            if (bCanLaunchTrap)
            {
                GameObject trap = Instantiate(trapObject, transform.position, Quaternion.identity);
                ObstacleComponent obstacleComponent = trap.GetComponent<ObstacleComponent>();
                if (!obstacleComponent)
                {
                    Debug.LogError("Error: There is no obstacle component attached to the trap object prefab");
                    return;
                }
                obstacleComponent.SetObjectVelocity(trapTriggerDirection);
                bCanLaunchTrap = false;
                StartCoroutine(TrapRefresh());
            }
        }


    }

    IEnumerator TrapRefresh()
    {
        yield return new WaitForSeconds(spawnerRefreshRate);
        bCanLaunchTrap = true;
    }
}
