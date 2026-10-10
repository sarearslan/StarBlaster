using System.Collections;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject bossProjectilePrefab;
    [SerializeField] float projectileSpeed = 7f;
    [SerializeField] float projectileLifeTime = 6f;

    [Header("Fire Points")]
    [SerializeField] Transform leftFirePoint;
    [SerializeField] Transform centerFirePoint;
    [SerializeField] Transform rightFirePoint;

    [Header("Pattern Settings")]
    [SerializeField] float timeBetweenPatterns = 1.5f;
    [SerializeField] float timeBetweenBurstShots = 0.15f;
    [SerializeField] float spreadAngle = 18f;

    Coroutine attackCoroutine;
    AudioManager audioManager;

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void StartFiring()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public void StopFiring()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (FindAnyObjectByType<PlayerController>() == null)
            {
                attackCoroutine = null;
                yield break;
            }

            yield return StartCoroutine(
                TwinShotPattern()
            );

            yield return new WaitForSeconds(
                timeBetweenPatterns
            );

            if (FindAnyObjectByType<PlayerController>() == null)
            {
                attackCoroutine = null;
                yield break;
            }

            yield return StartCoroutine(
                SpreadShotPattern()
            );

            yield return new WaitForSeconds(
                timeBetweenPatterns
            );

            if (FindAnyObjectByType<PlayerController>() == null)
            {
                attackCoroutine = null;
                yield break;
            }

            yield return StartCoroutine(
                AimedShotPattern()
            );

            yield return new WaitForSeconds(
                timeBetweenPatterns
            );
        }
    }

    IEnumerator TwinShotPattern()
    {
        for (int i = 0; i < 3; i++)
        {
            FireProjectile(leftFirePoint, Vector2.down);
            FireProjectile(rightFirePoint, Vector2.down);

            PlayShootingSound();

            yield return new WaitForSeconds(
                timeBetweenBurstShots
            );
        }
    }

    IEnumerator SpreadShotPattern()
    {
        float[] angles =
        {
            -spreadAngle * 2f,
            -spreadAngle,
            0f,
            spreadAngle,
            spreadAngle * 2f
        };

        foreach (float angle in angles)
        {
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.down;

            FireProjectile(centerFirePoint, direction);
        }

        PlayShootingSound();

        yield return null;
    }

    IEnumerator AimedShotPattern()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();

        if (player == null)
        {
            yield break;
        }

        Transform[] firePoints =
        {
            leftFirePoint,
            centerFirePoint,
            rightFirePoint
        };

        for (int i = 0; i < 3; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                Vector2 direction =
                    (player.transform.position -
                     firePoint.position).normalized;

                FireProjectile(
                    firePoint,
                    direction
                );
            }

            PlayShootingSound();

            yield return new WaitForSeconds(
                timeBetweenBurstShots
            );
        }
    }

    void FireProjectile(Transform firePoint, Vector2 direction)
    {
        if (firePoint == null ||
            bossProjectilePrefab == null)
        {
            return;
        }

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg - 90f;

        GameObject projectile = Instantiate(
            bossProjectilePrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, angle)
        );

        Rigidbody2D projectileRB =
            projectile.GetComponent<Rigidbody2D>();

        projectileRB.linearVelocity =
            direction.normalized * projectileSpeed;

        Destroy(projectile, projectileLifeTime);
    }

    void PlayShootingSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayBossShooting();
        }
    }
}