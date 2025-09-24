using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // Este método se llama cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca el Text con el tag "ScoreText" en la nueva escena
        scoreText = GameObject.FindWithTag("ScoreText")?.GetComponent<Text>();

        // Si estamos en el menú, apaga el texto
        if (scene.name == "Menu")
        {
            if (scoreText != null)
                scoreText.gameObject.SetActive(false);
        }
        else
        {
            if (scoreText != null)
                scoreText.gameObject.SetActive(true);
            UpdateScoreUI();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddPoint()
    {
        Score++;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null && scoreText.gameObject.activeInHierarchy)
        {
            scoreText.text = "Score: " + Score;
        }
    }
}

