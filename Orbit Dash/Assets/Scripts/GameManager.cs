using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentScore = 0;
    private int highScore = 0;
    public bool isGameActive = false;

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

        // Load high score from previous session
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void StartGame()
    {
        currentScore = 0;
        isGameActive = true;

        // UI'yi güncelleme
        CanvasManager.Instance.UpdateScoreUI(currentScore);
        // Diðer oyun baþlangýcýyla ilgili iþlemler
    }

    public void EndGame()
    {
        isGameActive = false;

        // Eðer mevcut skor yüksek skordan büyükse, yeni yüksek skoru kaydet
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Oyunun bittiðini Canvas'a bildir
        // CanvasManager.Instance.ShowGameOverScreen(currentScore, highScore);
    }

    // Skor eklemek için kullanýlacak fonksiyon
    public void AddScore(int value)
    {
        if (!isGameActive) return;

        Debug.Log("Score added");
        currentScore += value;
        CanvasManager.Instance.UpdateScoreUI(currentScore);  // Skoru güncelle
    }

    // Skoru döndüren fonksiyon
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Yüksek skoru döndüren fonksiyon
    public int GetHighScore()
    {
        return highScore;
    }

    // Oyunu yeniden baþlatma
    public void RestartGame()
    {
        StartGame();
        // Oyun deðiþkenlerini sýfýrlama iþlemleri buraya eklenebilir
    }

    // Oyunu kapatma iþlemi
    public void QuitGame()
    {
        Application.Quit();
    }
}
