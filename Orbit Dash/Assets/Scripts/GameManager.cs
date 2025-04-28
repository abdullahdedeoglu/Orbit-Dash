using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentScore = 0;
    private int highScore = 0;
    public bool isGameActive = false;
    public bool isTutorialCompleted = false;

    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public bool isPaused = false;

    public GameObject cameraHolder;

    private int gameOverCount = 0;
    private bool adIsShowing = false;

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

        gameOverCount = PlayerPrefs.GetInt("GameOverCount", 0); // PlayerPrefs'ten oku
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }


    private void Start()
    {

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        isTutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        if (!isTutorialCompleted)
        {
            TutorialManager.Instance.StartTutorial();
        }
        else StartGame();
        
    }

    public void ActivateGame()
    {
        isGameActive = true;
        ActivateObject activateObject = cameraHolder.GetComponent<ActivateObject>();
        activateObject.enabled = true;

        if (!isPaused && isTutorialCompleted) // Tutorial tamamlandıysa ball spawnla
        {
            SpawnManager.Instance.SpawnBall(new Vector3(0, 0.6f, 0));
        }
    }

    public void StartGame()
    {
        //if (!isTutorialCompleted)
        //    ResetTheSettings();
        CanvasManager.Instance.StartCountdown(true); 
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

        gameOverCount++;
        PlayerPrefs.SetInt("GameOverCount", gameOverCount);
        PlayerPrefs.Save();

        if (gameOverCount % 2 == 0)
        {
            // Interstitial reklamı göster
            adIsShowing = true;
            FindObjectOfType<AdManager>().ShowInterstitial();
        }
        else
        {
            // Reklam yoksa direkt Game Over UI aç
            ShowGameOver();
        }
    }

    public void OnInterstitialAdClosed()
    {
        if (adIsShowing)
        {
            adIsShowing = false;
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        CanvasManager.Instance.ShowGameOverUI(currentScore, highScore);
    }

    public void AddScore(int value)
    {
        if (!isGameActive) return;

        currentScore += value;
        CanvasManager.Instance.UpdateScoreUI(currentScore);
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
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);

        CanvasManager.Instance.HideScoreAndPauseButton();
        CanvasManager.Instance.UpdateHighScoreUI(highScore);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);

        CanvasManager.Instance.ShowScoreAndPauseButton();
        CanvasManager.Instance.StartCountdown(false); // Pause sonrası devam etmek için
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    //private void ResetTheSettings()
    //{

    //}

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    public void ContinueButtonPressed()
    {
        FindObjectOfType<AdManager>().ShowRewarded(); // Reklam göster
    }

    // Bu çağrı AdManager'dan geldiğinde olacak
    public void ContinueAfterAd()
    {
        gameOverCanvas.SetActive(false);
        isGameActive = false;
        CanvasManager.Instance.ShowScoreAndPauseButton();
        SpawnManager.Instance.ClearAreaAround(new Vector3(0, 0.6f, 0), 0.6f);
        SpawnManager.Instance.SpawnBall(new Vector3(0, 0.6f, 0));

        CanvasManager.Instance.StartCountdown(true);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("GameOverCount", 0);
        PlayerPrefs.Save();
    }



}
