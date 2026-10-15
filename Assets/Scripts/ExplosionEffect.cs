
using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    public Sprite[] explosionSprites;
    public float frameDuration = 0.1f;
    public float finalScale = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startingScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingScale = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(PlayExplosion());
    }

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

            // Gradually grow as the explosion progresses
            float progress = (float)(i + 1) / explosionSprites.Length;

            transform.localScale = Vector3.Lerp(
                startingScale,
                startingScale * finalScale,
                progress
            );

            yield return new WaitForSeconds(frameDuration);
        }

        // Remove the explosion after the final frame
        Destroy(gameObject);
    }
}


