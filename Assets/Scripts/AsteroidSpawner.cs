
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int asteroidCount = 3;
    public float spawnPadding = 0.5f;

    void Start()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            SpawnAsteroid();
        }
    }

    public void SpawnAsteroid()
    {
        if (asteroidPrefab == null)
            return;

        Camera cam = Camera.main;

        // Choose a random screen edge
        int edge = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (edge)
        {
            case 0: // Left
                spawnPosition = new Vector2(
                    0.05f,
                    Random.Range(0.05f, 0.95f)
                );
                break;

            case 1: // Right
                spawnPosition = new Vector2(
                    0.95f,
                    Random.Range(0.05f, 0.95f)
                );
                break;

            case 2: // Bottom
                spawnPosition = new Vector2(
                    Random.Range(0.05f, 0.95f),
                    0.05f
                );
                break;

            case 3: // Top
                spawnPosition = new Vector2(
                    Random.Range(0.05f, 0.95f),
                    0.95f
                );
                break;
        }

        Vector3 worldPosition =
            cam.ViewportToWorldPoint(spawnPosition);

        worldPosition.z = 0f;

        Instantiate(
            asteroidPrefab,
            worldPosition,
            Quaternion.identity
        );
    }
}


