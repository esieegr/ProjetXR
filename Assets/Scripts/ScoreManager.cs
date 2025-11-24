using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public TMP_Text scoreText;
    
    public int remainingTargets = 0;
    public TMP_Text targetsText; // optional; assign a TMP Text in Canvas

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void SetTotalTargets(int total)
    {
        remainingTargets = total;
        UpdateUI();
    }

    public void TargetDestroyed()
    {
        remainingTargets--;
        if (remainingTargets < 0) remainingTargets = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
        if (targetsText != null)
        {
            if (remainingTargets == 0)
            {
                targetsText.text = "Victoire !";
            }
            else
            {
                targetsText.text = $"{remainingTargets} Restants";
            }
        }
    }
}
