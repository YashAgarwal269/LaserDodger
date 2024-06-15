using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 250f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private UnityEvent onHitTarget;

    [SerializeField] public Transform pointA; // First point of the area
    [SerializeField] public Transform pointB; // Second point of the area
    [SerializeField] private float speed = 1f; // Speed of the laser movement
    public bool isPlayerDead;
    public GameOverManager gameOverManager;

    private RaycastHit rayHit;
    private Ray ray;

    private float t = 0f; // Interpolation parameter
    private void Awake()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();

    }

    private void Start()
    {
        lineRenderer.positionCount = 2; // Set position count to 2
    }

    private void Update()
    {
        // Interpolate t between 0 and 1 over time
        t += speed * Time.deltaTime;
        if (t > 1f)
        {
            // Reverse direction
            t = 1f - (t - 1f);
            speed = -speed;
        }
        else if (t < 0f)
        {
            // Reverse direction
            t = -t;
            speed = -speed;
        }

        // Calculate the new position based on interpolation
        Vector3 newPosition = Vector3.Lerp(pointA.position, pointB.position, t);
        transform.position = newPosition;

        // Ray setup and detection
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~layerMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rayHit.point);

            // Check if the laser hits the player
            if (rayHit.collider.CompareTag("Player"))
            {
                // Handle player death (e.g., deactivate player object, show death animation)
                Debug.Log("Player hit by laser!");
                // You can add your player death logic here
                PlayerDeath();
            }
            else
            {
                if (rayHit.collider.TryGetComponent(out Target target))
                {
                    target.Hit();
                    onHitTarget.Invoke();
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, ray.direction * laserDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rayHit.point, 0.23f);
        }
    }

    private void PlayerDeath()
    {
        // Example logic for player death:
        // Deactivate player object, show death animation, etc.
        // For demonstration purposes, we deactivate the player object here
        if (rayHit.collider.CompareTag("Player"))
        {
            rayHit.collider.gameObject.SetActive(false); // Deactivate player object
            Debug.Log("Player died!");
            isPlayerDead = true;
           gameOverManager.ShowGameOverScreen();
        }
    }
    
}
