
using UnityEngine;
public class AsteroidMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 50f;
    public Sprite[] asteroidSprites;

    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;

    // Gets the asteroid's SpriteRenderer.
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Randomizes the asteroid's appearance and movement at startup.
    void Start()
    {
        RandomizeAppearance();
        RandomizeMovement();
    }

    // Moves, rotates, and wraps the asteroid around the screen.
    void Update()
    {
        transform.position +=
            (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        transform.Rotate(
            0f, 0f, rotationSpeed * Time.deltaTime
        );

        Vector3 position =
            Camera.main.WorldToViewportPoint(transform.position);

        bool wrapped = false;

        if (position.x < 0f)
        {
            position.x = 1f;
            wrapped = true;
        }
        else if (position.x > 1f)
        {
            position.x = 0f;
            wrapped = true;
        }

        if (position.y < 0f)
        {
            position.y = 1f;
            wrapped = true;
        }
        else if (position.y > 1f)
        {
            position.y = 0f;
            wrapped = true;
        }

        transform.position =
            Camera.main.ViewportToWorldPoint(position);

        if (wrapped)
            RandomizeMovement();
    }

    // Selects a random sprite for the asteroid.
    void RandomizeAppearance()
    {
        if (asteroidSprites != null &&
            asteroidSprites.Length > 0 &&
            spriteRenderer != null)
        {
            int index = Random.Range(0, asteroidSprites.Length);
            spriteRenderer.sprite = asteroidSprites[index];
        }
    }

    // Assigns a random movement direction and rotation speed.
    void RandomizeMovement()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        rotationSpeed = Random.Range(-50f, 50f);
    }
}




