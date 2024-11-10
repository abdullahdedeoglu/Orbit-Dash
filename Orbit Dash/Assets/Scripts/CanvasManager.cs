using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI deathText;

    public GameObject pauseButton;
    public Image pauseBackground;

    public GameObject gameOverCanvas;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    public TextMeshProUGUI countdownText; // Geri sayým için UI elementi

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
        countdownText.gameObject.SetActive(false); // Geri sayýmý gizle
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

    public void ShowGameOverUI(int currentScore, int highScore)
    {
        gameOverCanvas.SetActive(true);
        currentScoreText.text = "Score: " + currentScore;
        gameOverHighScoreText.text = "High Score: " + highScore;
        HideScoreAndPauseButton();
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

    public void StartCountdown(bool shouldRespawn)
    {
        StartCoroutine(StartCountdownCoroutine(shouldRespawn));
    }

    // Geri sayým fonksiyonu
    public IEnumerator StartCountdownCoroutine(bool shouldRespawn)
    {
        Time.timeScale = 0; // Geri sayým sýrasýnda oyun duracak
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f); // Oyun süresine baðlý kalmadan geri sayým
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1; // Geri sayým bitince oyun devam edecek

        // Yalnýzca oyun baþlatýldýðýnda veya öldükten sonra top spawnlanacak
        if (shouldRespawn)
        {
            GameManager.Instance.ActivateGame(); // Oyunu baþlat
        }
    }
}
