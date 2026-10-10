using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Base Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float baseFireRate = 0.2f;

    [Header("AI Variables")]
    [SerializeField] bool useAI;
    [SerializeField] float minimumFireRate = 0.2f;
    [SerializeField] float fireRateVariance = 0f;
    [HideInInspector] public bool isFiring;
    Coroutine fireCoroutine;
    AudioManager audioManager;

    [Header("Triple Shot")]
    [SerializeField] float tripleShotAngle = 20f;
    [SerializeField] float tripleShotDuration = 8f;

    bool tripleShotActive;
    Coroutine tripleShotCoroutine;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        if (useAI)
        {
            float initialWaitTime = Random.Range(
                baseFireRate - fireRateVariance,
                baseFireRate + fireRateVariance
            );

            initialWaitTime = Mathf.Clamp(
                initialWaitTime,
                minimumFireRate,
                float.MaxValue
            );

            yield return new WaitForSeconds(initialWaitTime);
        }

        while (isFiring)
        {
            if (useAI &&
                FindAnyObjectByType<PlayerController>() == null)
            {
                isFiring = false;
                fireCoroutine = null;
                yield break;
            }

            if (tripleShotActive && !useAI)
            {
                FireProjectile(-tripleShotAngle);
                FireProjectile(0f);
                FireProjectile(tripleShotAngle);
            }
            else
            {
                FireProjectile(0f);
            }

            if (audioManager != null)
            {
                if (useAI)
                {
                    audioManager.PlayEnemyShooting();
                }
                else
                {
                    audioManager.PlayPlayerShooting();
                }
            }

            float waitTime = Random.Range(
                baseFireRate - fireRateVariance,
                baseFireRate + fireRateVariance
            );

            waitTime = Mathf.Clamp(
                waitTime,
                minimumFireRate,
                float.MaxValue
            );

            yield return new WaitForSeconds(waitTime);
        }

        fireCoroutine = null;
    }

    void FireProjectile(float angle)
    {
        Quaternion rotation =
            transform.rotation * Quaternion.Euler(0f, 0f, angle);

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            rotation
        );

        Rigidbody2D projectileRB =
            projectile.GetComponent<Rigidbody2D>();

        Vector2 direction =
            Quaternion.Euler(0f, 0f, angle) * transform.up;

        projectileRB.linearVelocity =
            direction * projectileSpeed;

        Destroy(projectile, projectileLifeTime);
    }

    public void ActivateTripleShot()
    {
        if (tripleShotCoroutine != null)
        {
            StopCoroutine(tripleShotCoroutine);
        }

        tripleShotCoroutine =
            StartCoroutine(TripleShotTimer());
    }

    IEnumerator TripleShotTimer()
    {
        tripleShotActive = true;

        yield return new WaitForSeconds(tripleShotDuration);

        tripleShotActive = false;
        tripleShotCoroutine = null;
    }
}
