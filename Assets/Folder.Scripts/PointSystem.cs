using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PointSystem : MonoBehaviour
{
    public TMP_Text timerText;

    public GameFlowManager game;
    public string sceneName;

    public int requiredScore = 100;
    public float additionalTime = 60f;

    private float timeRemaining;
    private bool targetReached = false;

    void Start()
    {
        if (game == null)
        {
            Debug.LogError("GameFlowManager is not assigned!");
            return;
        }

        timeRemaining = game.roundTime;

        // Initialize score and timer display
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (game == null || targetReached) return;

        // Display current score

        // Check whether required score has been achieved
        if (game.currentScore >= requiredScore)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        // Countdown timer
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0, timeRemaining);
        }

        // Check if time has run out
        if (timeRemaining <= 0)
        {
            if (game.currentScore < requiredScore)
            {
                // Add another 60 seconds
                timeRemaining += additionalTime;

                Debug.Log("Time extended! Another 60 seconds added.");
            }
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        timerText.text = string.Format(
            "TIME: {0:00}:{1:00}",
            minutes,
            remainingSeconds
        );
    }
}
