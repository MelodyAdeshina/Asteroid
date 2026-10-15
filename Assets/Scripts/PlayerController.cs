
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    public float respawnDelay = 0.6f;

    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public InputActionAsset controls;

    private InputAction moveAction;
    private InputAction rotateAction;
    private InputAction shootAction;
    private bool isRespawning = false;

    // Gets the player's input actions.
    void Awake()
    {
        var playerMap = controls.FindActionMap("Player");

        moveAction = playerMap.FindAction("Move");
        rotateAction = playerMap.FindAction("Rotate");
        shootAction = playerMap.FindAction("Shoot");
    }

    // Enables player controls.
    void OnEnable()
    {
        moveAction?.Enable();
        rotateAction?.Enable();
        shootAction?.Enable();
    }

    // Disables player controls.
    void OnDisable()
    {
        moveAction?.Disable();
        rotateAction?.Disable();
        shootAction?.Disable();
    }

    // Handles movement, rotation, shooting, and screen wrapping.
    void Update()
    {
        if (isRespawning)
            return;

        float move = moveAction.ReadValue<float>();
        float turn = rotateAction.ReadValue<float>();

        transform.Rotate(
            0f, 0f,
            -turn * rotationSpeed * Time.deltaTime
        );

        if (move > 0f)
        {
            transform.position +=
                transform.up * moveSpeed * Time.deltaTime;
        }

        if (shootAction != null &&
            shootAction.WasPressedThisFrame())
        {
            Shoot();
        }

        WrapScreen();
    }

    // Creates a bullet and sets its travel direction.
    void Shoot()
    {
        if (bulletPrefab == null || shootingPoint == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootingPoint.position,
            shootingPoint.rotation
        );

        bullet.transform.Rotate(0f, 0f, -90f);

        BulletMovement bulletMovement =
            bullet.GetComponent<BulletMovement>();

        if (bulletMovement != null)
            bulletMovement.SetDirection(transform.up);
    }

    // Starts the respawn process when the player hits an asteroid.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Asteroid") ||
            isRespawning)
        {
            return;
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.LoseLife();

        StartCoroutine(RespawnPlayer());
    }

    // Hides the player temporarily, then restores it at the center.
    IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        moveAction?.Disable();
        rotateAction?.Disable();
        shootAction?.Disable();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        moveAction?.Enable();
        rotateAction?.Enable();
        shootAction?.Enable();

        isRespawning = false;
    }

    // Wraps the player to the opposite screen edge.
    void WrapScreen()
    {
        Vector3 position =
            Camera.main.WorldToViewportPoint(transform.position);

        if (position.x < 0f)
            position.x = 1f;
        else if (position.x > 1f)
            position.x = 0f;

        if (position.y < 0f)
            position.y = 1f;
        else if (position.y > 1f)
            position.y = 0f;

        transform.position =
            Camera.main.ViewportToWorldPoint(position);
    }
}




