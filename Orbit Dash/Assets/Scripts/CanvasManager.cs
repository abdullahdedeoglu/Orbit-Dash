using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI deathText;

    public GameObject pauseButton;
    public Image pauseBackground;

    public GameObject gameOverCanvas; // Yeni game over canvas
    public TextMeshProUGUI currentScoreText; // Þu anki skor metni
    public TextMeshProUGUI gameOverHighScoreText; // Game over ekranýnda yüksek skor

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateScoreUI(0);
        highScoreText.gameObject.SetActive(false);
        pauseBackground.color = new Color(0, 0, 0, 0.5f);
    }

    public void UpdateScoreUI(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    public void UpdateHighScoreUI(int highScore)
    {
        highScoreText.text = "High Score: " + highScore;
        highScoreText.gameObject.SetActive(true);
    }

    public void HideScoreAndPauseButton()
    {
        scoreText.gameObject.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void ShowScoreAndPauseButton()
    {
        scoreText.gameObject.SetActive(true);
        pauseButton.SetActive(true);
    }

    // Game Over ekranýný gösteren fonksiyon
    public void ShowGameOverUI(int currentScore, int highScore)
    {
        gameOverCanvas.SetActive(true);
        currentScoreText.text = "Score: " + currentScore;
        gameOverHighScoreText.text = "High Score: " + highScore;
        HideScoreAndPauseButton(); // Skor ve pause butonlarýný gizle
    }

    public void OnPauseButtonPressed()
    {
        if (GameManager.Instance.isPaused)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            GameManager.Instance.PauseGame();
        }
    }
}
