
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private GameObject nextLevelButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip levelCompleteSound;
    [SerializeField] private AudioClip failSound;

    private LaunchController launchController;
    private bool levelEnded;

    private void Start()
    {
        launchController = FindFirstObjectByType<LaunchController>();
        resultPanel.SetActive(false);
    }

    private void Update()
    {
        if (launchController != null && launchController.isStuck)
        {
            FailLevel();
        }
    }

    public void WinLevel()
    {
        if (levelEnded) return;
        levelEnded = true;
        if (levelCompleteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(levelCompleteSound);
        }
        if (SceneManager.GetActiveScene().name == "Level_05")
        {
            resultText.text = "GAME COMPLETE!";
        }
        else
        {
            resultText.text = "LEVEL COMPLETE!";
        }
        if (nextLevelButton != null) nextLevelButton.SetActive(true);
        resultPanel.SetActive(true);
    }

    public void FailLevel()
    {
        if (levelEnded) return;
        levelEnded = true;
        if (failSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(failSound);
        }
        Handheld.Vibrate();
        resultText.text = "LEVEL FAILED!";
        if (nextLevelButton != null) nextLevelButton.SetActive(false);
        resultPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
