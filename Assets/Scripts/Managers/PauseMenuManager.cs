using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private PlayerInput playerInput; 
    private PlayerInput.UIActions uiActions;

    private bool isPaused = false;

    void Awake()
    {
        playerInput = new PlayerInput();
        uiActions = playerInput.UI;

        uiActions.Pause.performed += ctx => TogglePauseMenu();
    }

    private void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }

    public void SaveAndQuit()
    {
        //Save Logic needs to go here
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void OnEnable()
    {
        uiActions.Enable();
    }

    void OnDisable()
    {
        uiActions.Disable();
    }
}
