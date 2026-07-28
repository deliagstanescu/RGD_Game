using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject settingsCanvas;

    private bool isMenuOpen = false;

    void Start()
    {
        CloseMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    public void OpenMenu()
    {
        isMenuOpen = true;
        if (settingsCanvas != null)
            settingsCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //pauza la joc
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        if (settingsCanvas != null)
            settingsCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}