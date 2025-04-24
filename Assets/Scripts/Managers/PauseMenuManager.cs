using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private PlayerInput playerInput; 
    private PlayerInput.UIActions uiActions;
    public PlayerHealth playerHealth;

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
    if (SaveSystem.Instance == null)
    {
        Debug.LogError("SaveSystem.Instance is null!");
        return;
    }
    if (GlobalReferences.Instance == null)
    {
        Debug.LogError("GlobalReferences.Instance is null!");
        return;
    }
    if (playerHealth == null)
    {
        Debug.LogError("playerHealth is null!");
        return;
    }

    SaveSystem.Instance.SavePlayerData(GlobalReferences.Instance, playerHealth);

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
