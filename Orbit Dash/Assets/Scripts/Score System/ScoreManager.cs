using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score;

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

    public void AddScore(int amount)
    {
        score += amount;
        CanvasManager.Instance.UpdateScoreUI(score); 
    }

    public int GetScore()
    {
        return score;
    }
}

