using TMPro;
using UnityEngine;

public class UIEndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper =
            FindAnyObjectByType<ScoreKeeper>();

        if (scoreKeeper != null &&
            scoreText != null)
        {
            scoreText.text =
                "FINAL SCORE:\n" +
                scoreKeeper.GetScore();
        }
    }
}