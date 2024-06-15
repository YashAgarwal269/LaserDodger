using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Button restartButton;

    private void Start()
    {
        // Ensure Game Over screen is initially disabled
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        // Hook up restart button onClick listener
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void ShowGameOverScreen()
    {
        // Activate Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Optionally, pause game or perform other actions
        Time.timeScale = 0f; // Pause game time
    }

    public void RestartGame()
    {
        // Reload the current scene (assuming it's the main game scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Resume time scale if paused
        Time.timeScale = 1f;
    }
}
