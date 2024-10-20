using TMPro;
using UnityEngine;
using UnityEngine.UI; // Transparan arka plan için

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText; // Yüksek skor metni
    public TextMeshProUGUI deathText;

    public GameObject pauseButton; // Pause butonunu referans al
    public Image pauseBackground; // Pause ekraný arka planý için transparan efekt

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
        deathText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false); // Ýlk baþta gizli
        pauseBackground.color = new Color(0, 0, 0, 0.5f); // Transparan siyah arka plan
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

    public void ShowDeathMessage()
    {
        deathText.text = "Game Over!";
        deathText.gameObject.SetActive(true);
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
