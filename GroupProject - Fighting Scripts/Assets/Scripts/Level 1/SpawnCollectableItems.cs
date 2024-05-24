using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class HealthCollectibleSpawner : MonoBehaviour
{
    public GameObject healthCollectiblePrefab; // Reference to the health collectible prefab
    public float minSpawnTime = 5f; // Minimum time between spawns
    public float maxSpawnTime = 10f; // Maximum time between spawns

    private GameObject currentHealthCollectible; // Reference to the currently spawned health collectible
    private float nextSpawnTime; // Time when the next spawn will occur
    public Transform[] healthObjects;
    public int healthObjectsCount = 4;
    void Start()
    {
        // Initialize nextSpawnTime with a random value within the range
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        // Check if it's time to spawn a new health collectible
        if (Time.time >= nextSpawnTime)
        {
            DestroyPreviousHealthCollectible(); // Destroy the previous health collectible
            SpawnHealthCollectible(); // Spawn the new health collectible
            CalculateNextSpawnTime(); // Calculate when the next spawn will occur
        }
    }

    void SpawnHealthCollectible()
    {

        Transform randomPosition = healthObjects[Random.Range(0, healthObjects.Length)];
        currentHealthCollectible = Instantiate(healthCollectiblePrefab, randomPosition.position, Quaternion.identity);

        // Instantiate the health collectible prefab at a random position within the game area
        //Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f)); // Adjust position range as per your game area
        //currentHealthCollectible = Instantiate(healthCollectiblePrefab, spawnPosition, Quaternion.identity);
    }

    void DestroyPreviousHealthCollectible()
    {
        // Check if there's a previously spawned health collectible, and destroy it
        if (currentHealthCollectible != null)
        {
            Destroy(currentHealthCollectible);
        }
    }

    void CalculateNextSpawnTime()
    {
        // Calculate the next spawn time based on the defined range
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}