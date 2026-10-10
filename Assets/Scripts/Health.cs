using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int scoreValue = 50;
    [SerializeField] int health = 50;
    [SerializeField] ParticleSystem hitParticles;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;
    AudioManager audioManager;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    int maximumHealth;


    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioManager = FindAnyObjectByType<AudioManager>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        levelManager = FindAnyObjectByType<LevelManager>();
        maximumHealth = health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
            PlayHitParticles();
            damageDealer.Hit();
            if (audioManager != null)
            {
                if (GetComponent<BossDeath>() != null)
                {
                    audioManager.PlayBossDamage();
                }
                else
                {
                    audioManager.PlayNormalDamage();
                }
            }

            if (applyCameraShake)
            {
                cameraShake.Play();
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isPlayer)
        {
            BossShooter bossShooter =
                FindAnyObjectByType<BossShooter>();

            if (bossShooter != null)
            {
                bossShooter.StopFiring();
            }
            if (audioManager != null)
            {
                audioManager.PlayNormalExplosion();
            }

            StartCoroutine(LoadGameOverAfterDelay());
            return;
        }

        if (scoreKeeper != null)
        {
            scoreKeeper.ModifyScore(scoreValue);
        }

        BossDeath bossDeath =
            GetComponent<BossDeath>();

        if (bossDeath != null)
        {
            bossDeath.PlayDeath();
            return;
        }

        Destroy(gameObject);
    }

    void PlayHitParticles()
    {
        if (hitParticles != null)
        {
            ParticleSystem particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
            Destroy(particles, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        health = Mathf.Clamp(health, 0, maximumHealth);
    }

    IEnumerator LoadGameOverAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");
    }
}
