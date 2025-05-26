using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverScreen;
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverScreen.SetActive(false);
        Health.totalHealth = 1f;
        Scoring.totalScore = 0;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + Scoring.totalScore;
    }
}
