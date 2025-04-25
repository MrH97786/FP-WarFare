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

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int waveSurvived = GlobalReferences.Instance.waveNumber;
            if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore(currentSceneIndex))
            {
                SaveLoadManager.Instance.SaveHighScore(currentSceneIndex, waveSurvived - 1);
            }

            int highestPoints = GlobalReferences.Instance.scoreNumber;
            if (highestPoints > SaveLoadManager.Instance.LoadHighPointScore(currentSceneIndex))
            {
                SaveLoadManager.Instance.SaveHighPointScore(currentSceneIndex, highestPoints);
            }


            StartCoroutine(ReturnToMainMenu(4f));
        }
    }


    private IEnumerator ReturnToMainMenu(float delayBeforeReturn)
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 2) // Level 1
        {
            SceneManager.LoadScene(3); // Load Level 2
        }
        else if (currentSceneIndex == 3) // Level 2
        {
            SceneManager.LoadScene(0); // Load Main Menu
        }
    }

}
