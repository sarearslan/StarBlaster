using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Level 1")]
    [SerializeField] WaveConfigSO[] level1Waves;

    [Header("Level 2")]
    [SerializeField] WaveConfigSO[] level2Waves;

    [Header("Settings")]
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] bool isLooping;

    WaveConfigSO currentWave;
    Coroutine spawnCoroutine;

    void Start()
    {
        StartLevel(1);
    }

    public void StartLevel(int levelNumber)
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        WaveConfigSO[] selectedWaves;

        if (levelNumber == 1)
        {
            selectedWaves = level1Waves;
        }
        else
        {
            selectedWaves = level2Waves;
        }

        spawnCoroutine = StartCoroutine(SpawnWaves(selectedWaves));
    }

    IEnumerator SpawnWaves(WaveConfigSO[] waves)
    {
        if (waves == null || waves.Length == 0)
        {
            spawnCoroutine = null;
            yield break;
        }

        do
        {
            foreach (WaveConfigSO wave in waves)
            {
                if (wave == null)
                {
                    continue;
                }

                currentWave = wave;

                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(
                        currentWave.GetEnemyPrefab(i),
                        currentWave.GetStartingWayPoint().position,
                        Quaternion.identity,
                        transform
                    );

                    yield return new WaitForSeconds(
                        currentWave.GetRandomEnemySpawnTİme()
                    );
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        while (isLooping);

        spawnCoroutine = null;
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}