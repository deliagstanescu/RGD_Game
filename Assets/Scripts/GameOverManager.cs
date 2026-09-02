using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject deathScreenPanel;
    public TextMeshProUGUI reasonText;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);
    }

    public void TriggerGameOver(string reason = "")
    {
        if (isGameOver) return;
        isGameOver = true;
        if (reasonText != null)
            reasonText.text = reason;

        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}