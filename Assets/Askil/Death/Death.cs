using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
