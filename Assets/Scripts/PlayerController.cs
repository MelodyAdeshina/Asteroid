
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

    void Awake()
    {
        var playerMap = controls.FindActionMap("Player");

        moveAction = playerMap.FindAction("Move");
        rotateAction = playerMap.FindAction("Rotate");
        shootAction = playerMap.FindAction("Shoot");
    }

    void OnEnable()
    {
        moveAction?.Enable();
        rotateAction?.Enable();
        shootAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        rotateAction?.Disable();
        shootAction?.Disable();
    }

    void Update()
    {
        if (isRespawning)
            return;

        float move = moveAction.ReadValue<float>();
        float turn = rotateAction.ReadValue<float>();

        // Rotate the spaceship
        transform.Rotate(
            0f, 0f,
            -turn * rotationSpeed * Time.deltaTime
        );

        // Move forward
        if (move > 0f)
        {
            transform.position +=
                transform.up * moveSpeed * Time.deltaTime;
        }

        // Shoot when Space is pressed
        if (shootAction != null &&
            shootAction.WasPressedThisFrame())
        {
            Shoot();
        }

        WrapScreen();
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootingPoint == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootingPoint.position,
            shootingPoint.rotation
        );

        // Rotate the laser artwork so its tip faces forward
        bullet.transform.Rotate(0f, 0f, -90f);

        BulletMovement bulletMovement =
            bullet.GetComponent<BulletMovement>();

        if (bulletMovement != null)
        {
            bulletMovement.SetDirection(transform.up);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Asteroid")
            || isRespawning)
        {
            return;
        }
        if (ScoreManager.Instance != null)
        {
             ScoreManager.Instance.LoseLife();
             }
             StartCoroutine(RespawnPlayer());
             }


    IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        // Disable controls during respawn
        moveAction?.Disable();
        rotateAction?.Disable();
        shootAction?.Disable();

        // Hide the spaceship and disable its collider
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Wait before respawning
        yield return new WaitForSeconds(respawnDelay);

        // Return to the center, facing upward
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Make the spaceship visible and restore collisions
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        // Restore controls
        moveAction?.Enable();
        rotateAction?.Enable();
        shootAction?.Enable();

        isRespawning = false;
    }

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



