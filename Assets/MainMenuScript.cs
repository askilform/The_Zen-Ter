using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Blarg");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
