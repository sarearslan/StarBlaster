using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }


    public void LoadGame()
    {
        if (scoreKeeper != null)
        {
            scoreKeeper.ResetScore();
        }

        SceneManager.LoadScene("GameScene");
    }
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadYouWin()
    {
        SceneManager.LoadScene("YouWin");
    }
}
