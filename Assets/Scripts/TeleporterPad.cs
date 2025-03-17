using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TeleporterPad : MonoBehaviour
{
    public GameObject LevelCompleteUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<PlayerScreenBlackout>().StartFade();
            LevelCompleteUI.gameObject.SetActive(true);

            int waveSurvived = GlobalReferences.Instance.waveNumber;
            if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
            {
                SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
            }

            int highestPoints = GlobalReferences.Instance.scoreNumber;
            if (highestPoints > SaveLoadManager.Instance.LoadHighPointScore())
            {
                SaveLoadManager.Instance.SaveHighPointScore(highestPoints);
            }

            StartCoroutine(ReturnToMainMenu(4f));
        }
    }


    private IEnumerator ReturnToMainMenu(float delayBeforeReturn)
    {
        // Wait for a delay (e.g., to show the level complete message)
        yield return new WaitForSeconds(delayBeforeReturn);

        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
