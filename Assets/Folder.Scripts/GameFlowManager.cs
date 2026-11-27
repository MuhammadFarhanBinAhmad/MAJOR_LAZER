using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public int currentRound = 1;

    public int[] requiredPoints = { 10, 25, 50, 50, 30 };

    public float roundTime = 150f;

    public int currentScore;
    public float timeRemaining;

    public bool IsRoundActive { get; private set; }

    private string[] roundScenes =
    {
        "Round1",
        "Round2",
        "Round3",
        "Round4",
        "Round5"
    };

    private string[] cutsceneScenes =
    {
        "Cutscene 1",
        "Cutscene 2",
        "Cutscene 3",
        "Cutscene 4",
        "EndingCutscene"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!IsRoundActive) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            IsRoundActive = false;
        }
    }

    public void LoadCurrentRound()
    {
        SceneManager.LoadScene(roundScenes[currentRound - 1]);
    }

    public void BeginRound()
    {
        currentScore = 0;
        timeRemaining = roundTime;
        IsRoundActive = true;
    }

    public void AddPoints(int points)
    {
        if (!IsRoundActive) return;

        currentScore += points;

        if (currentScore >= requiredPoints[currentRound - 1])
        {
            CompleteRound();
        }
    }

    private void CompleteRound()
    {
        IsRoundActive = false;

        SceneManager.LoadScene(
            cutsceneScenes[currentRound - 1]
        );
    }

    public void ContinueToNextRound()
    {
        if (currentRound >= 5)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        currentRound++;

        LoadCurrentRound();
    }

    public void RetryRound()
    {
        LoadCurrentRound();
    }

    public void ReturnToMainMenu()
    {
        IsRoundActive = false;
        currentRound = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
