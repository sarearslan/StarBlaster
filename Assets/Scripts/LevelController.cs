using System.Collections;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] int level2RequiredScore = 2500;
    [SerializeField] int level3RequiredScore = 6000;

    [Header("Level UI")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] float levelTextDuration = 2f;

    [Header("Pickup Settings")]
    [SerializeField] GameObject tripleShotPickupPrefab;
    [SerializeField] Transform[] pickupSpawnPoints;
    [SerializeField] float minimumPickupSpawnTime = 10f;
    [SerializeField] float maximumPickupSpawnTime = 18f;

    [Header("Boss Settings")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform bossSpawnPoint;
    [SerializeField] Transform bossStopPoint;
    [SerializeField] float bossEntranceSpeed = 2f;

    [Header("Health Boost Settings")]
    [SerializeField] GameObject healthBoostPickupPrefab;
    [SerializeField] float minimumHealthSpawnTime = 15f;
    [SerializeField] float maximumHealthSpawnTime = 25f;

    Coroutine healthBoostSpawnCoroutine;

    ScoreKeeper scoreKeeper;
    EnemySpawner enemySpawner;

    int currentLevel;
    int lastPickupSpawnIndex = -1;

    bool isChangingLevel;

    Coroutine pickupSpawnCoroutine;

    void Start()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        enemySpawner = FindAnyObjectByType<EnemySpawner>();

        StartCoroutine(ShowLevelAndStart(1));
    }

    void Update()
    {
        if (scoreKeeper == null)
        {
            return;
        }

        int currentScore = scoreKeeper.GetScore();

        if (currentLevel == 1 &&
            !isChangingLevel &&
            currentScore >= level2RequiredScore)
        {
            StartCoroutine(ShowLevelAndStart(2));
        }
        else if (currentLevel == 2 &&
                 !isChangingLevel &&
                 currentScore >= level3RequiredScore)
        {
            StartCoroutine(ShowLevelAndStart(3));
        }
    }

    IEnumerator ShowLevelAndStart(int levelNumber)
    {
        isChangingLevel = true;
        currentLevel = levelNumber;

        if (levelNumber < 3)
        {
            enemySpawner.StartLevel(levelNumber);
        }
        else
        {
            enemySpawner.StopSpawning();

            AudioManager audioManager =
                FindAnyObjectByType<AudioManager>();

            if (audioManager != null)
            {
                audioManager.PlayBossFightMusic();
            }

            StartCoroutine(SpawnBoss());
        }

        if (levelNumber >= 2)
        {
            if (pickupSpawnCoroutine == null)
            {
                pickupSpawnCoroutine =
                    StartCoroutine(SpawnTripleShotPickups());
            }

            if (healthBoostSpawnCoroutine == null)
            {
                healthBoostSpawnCoroutine =
                    StartCoroutine(SpawnHealthBoostPickups());
            }
        }

        if (levelText != null)
        {
            levelText.text =
                "LEVEL " + levelNumber + "!";

            levelText.gameObject.SetActive(true);

            yield return new WaitForSeconds(
                levelTextDuration
            );

            levelText.gameObject.SetActive(false);
        }

        isChangingLevel = false;
    }

    IEnumerator SpawnTripleShotPickups()
    {
        while (currentLevel >= 2)
        {
            float waitTime = Random.Range(minimumPickupSpawnTime,
                                          maximumPickupSpawnTime
            );

            yield return new WaitForSeconds(waitTime);

            if (tripleShotPickupPrefab == null ||
                pickupSpawnPoints == null ||
                pickupSpawnPoints.Length == 0)
            {
                continue;
            }

            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, pickupSpawnPoints.Length
                );
            }
            while (pickupSpawnPoints.Length > 1 &&
                   randomIndex == lastPickupSpawnIndex);

            lastPickupSpawnIndex = randomIndex;

            Transform selectedSpawnPoint = pickupSpawnPoints[randomIndex];

            if (selectedSpawnPoint != null)
            {
                Instantiate(tripleShotPickupPrefab,
                            selectedSpawnPoint.position,
                            Quaternion.identity
                );
            }
        }

        pickupSpawnCoroutine = null;
    }

    IEnumerator SpawnBoss()
    {
        if (bossPrefab == null ||
            bossSpawnPoint == null ||
            bossStopPoint == null)
        {
            yield break;
        }

        GameObject boss = Instantiate(bossPrefab,
                                      bossSpawnPoint.position,
                                      Quaternion.identity
        );

        BossShooter bossShooter = boss.GetComponentInChildren<BossShooter>();

        Collider2D bossCollider = boss.GetComponent<Collider2D>();

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        while (Vector3.Distance(
               boss.transform.position,
               bossStopPoint.position
               ) > 0.01f)
        {
            boss.transform.position =Vector3.MoveTowards(
                                     boss.transform.position,
                                     bossStopPoint.position,
                                     bossEntranceSpeed * Time.deltaTime
                );

            yield return null;
        }

        boss.transform.position =
            bossStopPoint.position;

        if (bossCollider != null)
        {
            bossCollider.enabled = true;
        }

        if (bossShooter != null)
        {
            bossShooter.StartFiring();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetNextLevelRequiredScore()
    {
        if (currentLevel == 1)
        {
            return level2RequiredScore;
        }

        if (currentLevel == 2)
        {
            return level3RequiredScore;
        }
        return -1;
    }

    public int GetCurrentLevelStartingScore()
    {
        if (currentLevel == 1)
        {
            return 0;
        }

        if (currentLevel == 2)
        {
            return level2RequiredScore;
        }

        return level3RequiredScore;
    }
    
    IEnumerator SpawnHealthBoostPickups()
    {
        while (currentLevel >= 2)
        {
            float waitTime = Random.Range(
                minimumHealthSpawnTime,
                maximumHealthSpawnTime
            );

            yield return new WaitForSeconds(waitTime);

            if (healthBoostPickupPrefab == null ||
                pickupSpawnPoints == null ||
                pickupSpawnPoints.Length == 0)
            {
                continue;
            }

            int randomIndex = Random.Range(
                0,
                pickupSpawnPoints.Length
            );

            Transform selectedSpawnPoint =
                pickupSpawnPoints[randomIndex];

            if (selectedSpawnPoint != null)
            {
                Instantiate(
                    healthBoostPickupPrefab,
                    selectedSpawnPoint.position,
                    Quaternion.identity
                );
            }
        }

        healthBoostSpawnCoroutine = null;
    }
}