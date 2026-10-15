
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifetime = 2f;
    public GameObject[] explosionPrefabs;

    private Vector3 travelDirection;

    // Sets the direction the bullet travels.
    public void SetDirection(Vector3 direction)
    {
        travelDirection = direction.normalized;
    }

    // Schedules the bullet for destruction after its lifetime expires.
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Moves the bullet in its assigned direction.
    void Update()
    {
        transform.position +=
            travelDirection * bulletSpeed * Time.deltaTime;
    }

    // Handles asteroid hits, explosions, replacement spawning, and scoring.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            // Spawn a random explosion at the asteroid's position.
            if (explosionPrefabs != null &&
                explosionPrefabs.Length > 0)
            {
                int index = Random.Range(
                    0, explosionPrefabs.Length
                );

                Instantiate(
                    explosionPrefabs[index],
                    other.transform.position,
                    Quaternion.identity
                );
            }

            // Spawn a replacement asteroid.
            AsteroidSpawner spawner =
                FindFirstObjectByType<AsteroidSpawner>();

            if (spawner != null)
                spawner.SpawnAsteroid();

            Destroy(other.gameObject);
            Destroy(gameObject);

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddPoints(10);
        }
    }
}

