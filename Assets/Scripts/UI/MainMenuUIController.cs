using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Pressed!");
    }
}