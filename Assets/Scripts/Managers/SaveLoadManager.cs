using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    string highScoreKey = "BestWaveSavedValue";
    string highPointKey = "BestScorePointsSavedValue";

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

        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey, score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }
    }

    public void SaveHighPointScore(int score)
    {
        PlayerPrefs.SetInt(highPointKey, score);
    }

    public int LoadHighPointScore()
    {
        if (PlayerPrefs.HasKey(highPointKey))
        {
            return PlayerPrefs.GetInt(highPointKey);
        }
        else
        {
            return 0;
        }
    }
}
