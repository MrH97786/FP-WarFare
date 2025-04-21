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

    public void ExitApplication()
    {
        Application.Quit();
    }
}
