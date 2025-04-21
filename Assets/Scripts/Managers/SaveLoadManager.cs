using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

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

    public void SaveHighScore(int level, int score)
    {
        PlayerPrefs.SetInt($"HighScore_Level{level}", score);
    }

    public int LoadHighScore(int level)
    {
        return PlayerPrefs.GetInt($"HighScore_Level{level}", 0);
    }

    public void SaveHighPointScore(int level, int score)
    {
        PlayerPrefs.SetInt($"HighPoint_Level{level}", score);
    }

    public int LoadHighPointScore(int level)
    {
        return PlayerPrefs.GetInt($"HighPoint_Level{level}", 0);
    }
}
