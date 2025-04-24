using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText;

    int score = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Load score from the saved data if available
        if (SaveLoadManager.Instance.cachedScore != -1)
        {
            score = SaveLoadManager.Instance.cachedScore;
        }
        GlobalReferences.Instance.scoreNumber = score;
        scoreText.text = score.ToString();
    }

    public void AddPoints()
    {
        score += 1000;
        GlobalReferences.Instance.scoreNumber = score;
        scoreText.text = score.ToString();
    }

    public bool HasEnoughPoints(int amount)
    {
        return score >= amount;
    }

    public void DeductPoints(int amount)
    {
        if (HasEnoughPoints(amount))
        {
            score -= amount;
            GlobalReferences.Instance.scoreNumber = score;
            scoreText.text = score.ToString();
        }
    }
}
