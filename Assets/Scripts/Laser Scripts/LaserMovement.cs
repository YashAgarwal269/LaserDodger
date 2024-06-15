using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float initialSpeed = 1f; // Initial movement speed of the laser
    private float currentSpeed; // Current speed of the laser
    private bool movingTowardsB = true; // Flag to track laser direction
    private Transform pointA;
    private Transform pointB;

    public void Initialize(Transform start, Transform end, Vector3 rotation, float moveSpeed)
    {
        pointA = start;
        pointB = end;
        currentSpeed = initialSpeed;
        transform.position = pointA.position;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void Update()
    {
        MoveLaser();
    }

    private void MoveLaser()
    {
        if (pointA == null || pointB == null)
        {
            return;
        }

        Vector3 targetPosition = movingTowardsB ? pointB.position : pointA.position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        // Check if the laser has reached the target position closely enough
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Destroy the laser after completing its movement
            Destroy(gameObject);
        }
    }

    public void IncreaseSpeed(float amount)
    {
        currentSpeed += amount;
    }
}
