using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI deathText; 

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
    }

    public void UpdateScoreUI(int newScore)
    {
        scoreText.text = "Skor: " + newScore;
    }

    public void ShowDeathMessage()
    {
        deathText.text = "Game Over!";
        deathText.gameObject.SetActive(true);
    }
}
