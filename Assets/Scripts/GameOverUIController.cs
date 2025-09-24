using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        ScoreManager.Instance?.ResetScore();

        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName); // todo vuelve a Awake/OnEnable
    }

    public void MainMenu()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        ScoreManager.Instance?.ResetScore();
        SceneManager.LoadScene("Menu");
    }

    public void Level()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        ScoreManager.Instance?.ResetScore();
        SceneManager.LoadScene("Prueba");
    }
}