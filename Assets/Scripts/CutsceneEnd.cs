using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.UIActions uiActions;

    private bool hasSkipped = false;

    void Awake()
    {
        playerInput = new PlayerInput();
        uiActions = playerInput.UI;

        uiActions.Skip.performed += ctx => SkipCutscene();
    }

    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }

    private void SkipCutscene()
    {
        if (!hasSkipped)
        {
            hasSkipped = true;
            SceneManager.LoadScene("FP-WarFare S1");
        }
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(45f);

        if (!hasSkipped)
        {
            SceneManager.LoadScene("FP-WarFare S1");
        }
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
