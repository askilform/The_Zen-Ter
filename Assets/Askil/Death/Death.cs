using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private Timer timerScript;

    private void Start()
    {
        timerScript = GameObject.Find("TimerTXT").GetComponent<Timer>();
        timerScript.EndGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Go"))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
