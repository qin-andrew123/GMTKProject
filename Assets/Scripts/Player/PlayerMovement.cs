using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private GameObject turretComponent;
    [SerializeField] private float rotationSpeed = 1.0f;

    private Vector2 moveDirection = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        if (!turretComponent)
        {
            return;
        }

        Vector2 turretDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - turretComponent.transform.position;

        float turretAngle = Mathf.Atan2(turretDirection.y, turretDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion turretRotation = Quaternion.AngleAxis(turretAngle, Vector3.forward);
        turretComponent.transform.rotation = Quaternion.Slerp(turretComponent.transform.rotation, turretRotation, rotationSpeed * Time.deltaTime);
  
        float xAxisValue = Input.GetAxisRaw("Horizontal");
        float yAxisValue = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(xAxisValue, yAxisValue);
        moveDirection.Normalize();
        if(moveDirection == Vector2.zero)
        {
            return;
        }
        float MainBodyRotationAngle = Mathf.Atan2(yAxisValue, xAxisValue) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(MainBodyRotationAngle, Vector3.forward), movementSpeed * Time.deltaTime);
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);

    }
}
