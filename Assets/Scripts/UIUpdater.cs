using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Health playerHealth;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Level Progress")]
    [SerializeField] TextMeshProUGUI levelProgressText;

    LevelController levelController;

    void Start()
    {
        scoreKeeper =
            FindAnyObjectByType<ScoreKeeper>();

        levelController =
            FindAnyObjectByType<LevelController>();

        if (playerHealth != null &&
            healthSlider != null)
        {
            healthSlider.maxValue =
                playerHealth.GetHealth();

            healthSlider.value =
                playerHealth.GetHealth();
        }
    }
    void Update()
    {
        if (scoreKeeper != null &&
            scoreText != null)
        {
            scoreText.text =
                scoreKeeper.GetScore()
                    .ToString("000000000");
        }

        if (playerHealth != null)
        {
            healthSlider.value =
                playerHealth.GetHealth();
        }
        else
        {
            healthSlider.value = 0;
        }

        if (levelController != null &&
            levelProgressText != null)
        {
            UpdateLevelProgress();
        }
    }
    void UpdateLevelProgress()
    {
        int currentLevel = levelController.GetCurrentLevel();
        int currentScore = scoreKeeper.GetScore();

        int startingScore =
            levelController.GetCurrentLevelStartingScore();

        int requiredScore =
            levelController.GetNextLevelRequiredScore();

        levelProgressText.gameObject.SetActive(true);

        if (requiredScore == -1)
        {
            levelProgressText.text = "MAX LEVEL";
            return;
        }

        float progress = Mathf.InverseLerp(
            startingScore,
            requiredScore,
            currentScore
        );

        int progressPercent = Mathf.RoundToInt(
            progress * 100f
        );

        levelProgressText.text =
            "LEVEL " +
            currentLevel +
            "  •  " +
            progressPercent +
            "%";
    }
}
