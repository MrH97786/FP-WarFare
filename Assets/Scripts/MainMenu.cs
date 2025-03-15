using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    string newGameScene = "FP-WarFare S1";

    public AudioSource main_channel;
    public AudioClip background_music;

    void Start()
    {
        main_channel.clip = background_music; 
        main_channel.Play();
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Highest Wave Survived: {highScore}";
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
