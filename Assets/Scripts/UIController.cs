using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject puzzlePieces;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);

        startPanel.SetActive(true);
        puzzlePieces.SetActive(true);
        winPanel.SetActive(false);
        gamePanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ShowWin()
    {
        startPanel.SetActive(false);
        winPanel.SetActive(true);
        gamePanel.SetActive(false);
        puzzlePieces.SetActive(false);
        Time.timeScale = 0f;
    }

    private void StartGame()
    {
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        gamePanel.SetActive(true);
        puzzlePieces.SetActive(true);
        Time.timeScale = 1f;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
