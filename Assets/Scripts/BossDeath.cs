using System.Collections;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] float shakeDuration = 2f;
    [SerializeField] float shakeMagnitude = 0.15f;

    [Header("Explosion")]
    [SerializeField] ParticleSystem finalExplosion;
    [SerializeField] float winSceneDelay = 1f;

    bool isDying;

    LevelManager levelManager;
    AudioManager audioManager;

    void Awake()
    {
        levelManager =
            FindAnyObjectByType<LevelManager>();

        audioManager =
            FindAnyObjectByType<AudioManager>();
    }

    public void PlayDeath()
    {
        if (isDying)
        {
            return;
        }

        isDying = true;
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        BossShooter bossShooter = GetComponentInChildren<BossShooter>();

        Collider2D bossCollider = GetComponent<Collider2D>();

        Rigidbody2D bossRigidbody = GetComponent<Rigidbody2D>();

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (bossShooter != null)
        {
            bossShooter.StopFiring();
        }

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        if (bossRigidbody != null)
        {
            bossRigidbody.linearVelocity =
                Vector2.zero;
        }

        Vector3 originalPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;

            transform.position = originalPosition + (Vector3)randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        if (finalExplosion != null)
        {
            ParticleSystem explosion = Instantiate(finalExplosion,  transform.position,  Quaternion.identity);

            Destroy(explosion.gameObject, explosion.main.duration + explosion.main.startLifetime.constantMax);
        }

        if (audioManager != null)
        {
            audioManager.PlayBossExplosion();
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }

        yield return new WaitForSeconds(winSceneDelay);

        if (levelManager != null)
        {
            levelManager.LoadYouWin();
        }
    }
}