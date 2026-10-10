using UnityEngine;

public class HealthBoostPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 50;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float lifeTime = 12f;

    AudioManager audioManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(
            Vector3.down * moveSpeed * Time.deltaTime,
            Space.World
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            return;
        }

        Health playerHealth =
            player.GetComponent<Health>();

        if (playerHealth == null)
        {
            return;
        }

        playerHealth.Heal(healAmount);

        if (audioManager != null)
        {
            audioManager.PlayPickup();
        }

        Destroy(gameObject);
    }
}