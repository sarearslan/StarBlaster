using UnityEngine;

public class TripleShotPickup : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float lifeTime = 12f;
    AudioManager audioManager;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            return;
        }

        Shooter shooter =
            player.GetComponentInChildren<Shooter>();

        if (shooter == null)
        {
            return;
        }

        shooter.ActivateTripleShot();

        if (audioManager != null)
        {
            audioManager.PlayPickup();
        }

        Destroy(gameObject);
    }
}