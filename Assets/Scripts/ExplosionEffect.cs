
using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    public Sprite[] explosionSprites;
    public float frameDuration = 0.1f;
    public float finalScale = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startingScale;

    // Gets the SpriteRenderer and records the original scale.
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingScale = transform.localScale;
    }

    // Starts the explosion animation.
    void Start()
    {
        StartCoroutine(PlayExplosion());
    }

    // Plays each explosion frame, enlarges the effect, and destroys it.
    IEnumerator PlayExplosion()
    {
        if (explosionSprites == null ||
            explosionSprites.Length == 0 ||
            spriteRenderer == null)
        {
            Destroy(gameObject);
            yield break;
        }

        for (int i = 0; i < explosionSprites.Length; i++)
        {
            spriteRenderer.sprite = explosionSprites[i];

            float progress =
                (float)(i + 1) / explosionSprites.Length;

            transform.localScale = Vector3.Lerp(
                startingScale,
                startingScale * finalScale,
                progress
            );

            yield return new WaitForSeconds(frameDuration);
        }

        Destroy(gameObject);
    }
}



