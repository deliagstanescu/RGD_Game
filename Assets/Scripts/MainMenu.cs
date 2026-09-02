using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject startMenuPanel;

    [Header("Player")]
    public PlayerMovementBase playerMovement;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (startMenuPanel != null)
            startMenuPanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    public void StartGame()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);
        if (playerMovement != null)
            playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}