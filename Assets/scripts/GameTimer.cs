using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 600f; // 10 minutes in seconds
    public float currentTime;
    public TMP_Text timerText;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay(currentTime);
        }
        else
        {
            currentTime = 0;
            TimerEnded();
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        // Implement what happens when the timer ends
        SceneManager.LoadScene("VictoryScreen");
    }
}
