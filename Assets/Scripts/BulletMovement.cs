
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifetime = 2f;
    public GameObject[] explosionPrefabs; // Array of explosion prefabs
    private Vector3 travelDirection;

    // Receives the direction the spaceship is facing
    public void SetDirection(Vector3 direction)
    {
        travelDirection = direction.normalized;
    }

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet in the assigned direction
        transform.position +=
            travelDirection * bulletSpeed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            // Spawn a random explosion
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

            // Spawn a replacement asteroid
            AsteroidSpawner spawner =
                FindFirstObjectByType<AsteroidSpawner>();

            if (spawner != null)
            {
                spawner.SpawnAsteroid();
            }

            Destroy(other.gameObject);
            Destroy(gameObject);

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(10);
            }
        }
    }
}

//there is an error though. 'BulletMovement' does not contain a definition for 'SetDirection' and no accessible extension method 'SetDirection' accepting a first argument of type 'BulletMovement' could be found (are you missing a using directive or an assembly reference?)