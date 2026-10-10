using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 50f;

    [Header("Effects")]
    [SerializeField] ParticleSystem explosionParticles;

    Camera mainCamera;
    bool hasExploded;

    void Start()
    {
        mainCamera = Camera.main;

        rotationSpeed = Random.Range(
            -rotationSpeed,
            rotationSpeed
        );
    }

    void Update()
    {
        MoveAsteroid();
        CheckOutOfScreen();
    }

    void MoveAsteroid()
    {
        transform.Translate(
            Vector3.down * moveSpeed * Time.deltaTime,
            Space.World
        );

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );
    }

    void CheckOutOfScreen()
    {
        float bottomPosition =
            mainCamera.ViewportToWorldPoint(
                new Vector2(0f, 0f)
            ).y;

        if (transform.position.y < bottomPosition - 2f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();
        }

        if (player != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;

        if (explosionParticles != null)
        {
            ParticleSystem particles = Instantiate(
                explosionParticles,
                transform.position,
                Quaternion.identity
            );

            Destroy(
                particles.gameObject,
                particles.main.duration +
                particles.main.startLifetime.constantMax
            );
        }

        Destroy(gameObject);
    }
}