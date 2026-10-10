using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Prefabs")]
    [SerializeField] GameObject[] asteroidPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] float minimumSpawnTime = 5f;
    [SerializeField] float maximumSpawnTime = 10f;
    [SerializeField] float horizontalPadding = 1f;
    [SerializeField] float spawnHeightPadding = 1f;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            float waitTime = Random.Range(
                minimumSpawnTime,
                maximumSpawnTime
            );

            yield return new WaitForSeconds(waitTime);

            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefabs == null ||
            asteroidPrefabs.Length == 0)
        {
            return;
        }

        Vector2 bottomLeft =
            mainCamera.ViewportToWorldPoint(
                new Vector2(0f, 0f)
            );

        Vector2 topRight =
            mainCamera.ViewportToWorldPoint(
                new Vector2(1f, 1f)
            );

        float randomX = Random.Range(
            bottomLeft.x + horizontalPadding,
            topRight.x - horizontalPadding
        );

        Vector3 spawnPosition = new Vector3(
            randomX,
            topRight.y + spawnHeightPadding,
            0f
        );

        int randomIndex = Random.Range(
            0,
            asteroidPrefabs.Length
        );

        Instantiate(
            asteroidPrefabs[randomIndex],
            spawnPosition,
            Quaternion.identity,
            transform
        );
    }
}