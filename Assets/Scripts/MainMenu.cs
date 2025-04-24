using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text level1WaveText;
    public TMP_Text level1PointsText;
    public TMP_Text level2WaveText;
    public TMP_Text level2PointsText;


    string newGameScene = "FP-WarFare S1";

    public AudioSource main_channel;
    public AudioClip background_music;

    void Start()
    {
        main_channel.clip = background_music;
        main_channel.Play();

        level1WaveText.text = $"Level 1 - Highest Wave: {SaveLoadManager.Instance.LoadHighScore(1)}";
        level1PointsText.text = $"Level 1 - Highest Points: {SaveLoadManager.Instance.LoadHighPointScore(1)}";

        level2WaveText.text = $"Level 2 - Highest Wave: {SaveLoadManager.Instance.LoadHighScore(2)}";
        level2PointsText.text = $"Level 2 - Highest Points: {SaveLoadManager.Instance.LoadHighPointScore(2)}";
    }

    public void StartNewGame()
    {
        main_channel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ContinueGame()
    {
        PlayerData data = SaveSystem.Instance.LoadPlayerData();
        if (data != null)
        {
            GlobalReferences.Instance.waveNumber = data.waveNumber;
            GlobalReferences.Instance.scoreNumber = data.score;

            SaveLoadManager.Instance.cachedHealth = data.health;
            SaveLoadManager.Instance.cachedWave = data.waveNumber;
            SaveLoadManager.Instance.cachedScore = data.score;
        }

        SceneManager.LoadScene(newGameScene);
    }



    public void SelectLevel(int levelId)
    {
        string levelName = "";

        switch (levelId)
        {
            case 1:
                levelName = "FP-WarFare S1";
                break;
            case 2:
                levelName = "FP-WarFare S2";
                break;
            default:
                Debug.LogError("Invalid level ID selected: " + levelId);
                return;
        }

        SceneManager.LoadScene(levelName);
    }


    public void ExitApplication()
    {
        Application.Quit();
    }
}
