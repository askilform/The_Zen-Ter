using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timePlayed = 0;
    private TextMeshProUGUI timeTXT;

    public bool isDead = false;
    public GameObject parentTimer;

    private void Start()
    {
        timeTXT = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!isDead)
        {
            timePlayed += Time.deltaTime;
            timeTXT.text = timePlayed.ToString("F2");
        }
    }

    public void EndGame()
    {
        isDead = true;
        parentTimer.transform.localScale += new Vector3(1, 1, 1);
    }
}
