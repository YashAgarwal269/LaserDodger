using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointPair
{
    public Transform pointA;
    public Transform pointB;
    public Vector3 rotation;
}
public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab of the laser
    public PointPair[] pointPairs; // Array of point pairs for lasers to lerp between
    public float spawnInterval = 5f; // Interval to spawn new lasers
    public float initialLaserSpeed = 1f; // Initial speed of laser movement
    public float speedIncreaseRate = 0.1f; // Rate at which laser speed increases over time
    private int maxLasers = 5; // Maximum number of lasers allowed
    private List<GameObject> activeLasers = new List<GameObject>(); // List to track active lasers

    private void Start()
    {
        StartCoroutine(SpawnLaserWaves());
        StartCoroutine(IncreaseLaserSpeed());
    }

    private IEnumerator SpawnLaserWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (activeLasers.Count < maxLasers)
            {
                SpawnNewLaser();
            }
        }
    }

    private IEnumerator IncreaseLaserSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Adjust interval for speed increase
            foreach (GameObject laserObj in activeLasers)
            {
                if (laserObj != null)
                {
                    LaserMovement laserMovement = laserObj.GetComponent<LaserMovement>();
                    if (laserMovement != null)
                    {
                        laserMovement.IncreaseSpeed(speedIncreaseRate);
                    }
                }
            }
        }
    }

    private void SpawnNewLaser()
    {
        // Check for null references before spawning
        if (laserPrefab == null || pointPairs == null || pointPairs.Length < 1)
        {
            Debug.LogError("Cannot spawn new laser. Required references are not assigned.");
            return;
        }

        int pairIndex = activeLasers.Count % pointPairs.Length;
        PointPair pair = pointPairs[pairIndex];

        // Spawn the new laser at the pair's pointA position with the specified rotation
        GameObject newLaser = Instantiate(laserPrefab, pair.pointA.position, Quaternion.Euler(pair.rotation));
        if (newLaser == null)
        {
            Debug.LogError("Failed to instantiate laserPrefab.");
            return;
        }

        // Assign movement properties to the new laser
        LaserMovement newLaserScript = newLaser.GetComponent<LaserMovement>();
        if (newLaserScript != null)
        {
            newLaserScript.Initialize(pair.pointA, pair.pointB, pair.rotation, initialLaserSpeed);
            activeLasers.Add(newLaser);
        }
        else
        {
            Debug.LogError("The instantiated laser does not have the LaserMovement script attached.");
            Destroy(newLaser); // Clean up the incorrectly instantiated object
        }
    }
}
