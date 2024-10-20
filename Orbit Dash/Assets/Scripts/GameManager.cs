using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentScore = 0;
    private int highScore = 0;
    public bool isGameActive = false;

    public GameObject pauseCanvas;
    public GameObject gameOverCanvas; // Yeni game over canvas
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            StartGame();
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Start()
    {
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false); // Game over canvas baþlangýçta gizli
    }

    public void StartGame()
    {
        currentScore = 0;
        isGameActive = true;

        CanvasManager.Instance.UpdateScoreUI(currentScore);
    }

    public void EndGame()
    {
        isGameActive = false;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Game over ekranýný göster
        CanvasManager.Instance.ShowGameOverUI(currentScore, highScore);
    }

    public void AddScore(int value)
    {
        if (!isGameActive) return;

        currentScore += value;
        CanvasManager.Instance.UpdateScoreUI(currentScore);  // Skoru güncelle
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Oyun durduruluyor
        pauseCanvas.SetActive(true); // Pause canvas'ý gösteriliyor

        CanvasManager.Instance.HideScoreAndPauseButton();
        CanvasManager.Instance.UpdateHighScoreUI(highScore);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Oyun devam ediyor
        pauseCanvas.SetActive(false); // Pause canvas'ý gizleniyor

        CanvasManager.Instance.ShowScoreAndPauseButton();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    // Oynamaya devam et (ileride detaylýca yazacaðýz)
    public void ContinueGame()
    {
        isGameActive = true;
        Time.timeScale = 1;
        // Topu ortadan yeniden baþlat (ileride eklenecek)
        gameOverCanvas.SetActive(false); // Game over canvas'ý gizle
        CanvasManager.Instance.ShowScoreAndPauseButton(); // Skor ve pause butonunu geri getir
    }
}


