using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void ShowGameOver()
    {
        Time.timeScale=0;
        gameOverPanel.SetActive(true);        
    }

    public void RestartLevel()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale=1;
        SceneManager.LoadScene("HomeScene");
    }
}
