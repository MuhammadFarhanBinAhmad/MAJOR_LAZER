using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneFADE : MonoBehaviour
{
[Header("Scene Settings")]
    public string sceneName;

    [Header("Fade Settings")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    void Start()
    {
        // Fade in when the scene starts
        StartCoroutine(FadeIn());
    }

    public void LoadNextScene()
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    IEnumerator FadeIn()
    {
        fadePanel.alpha = 1f;
        fadePanel.blocksRaycasts = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadePanel.alpha = Mathf.Lerp(
                1f,
                0f,
                timer / fadeDuration
            );

            yield return null;
        }

        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
    }

    IEnumerator FadeOutAndLoad()
    {
        isTransitioning = true;

        fadePanel.blocksRaycasts = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadePanel.alpha = Mathf.Lerp(
                0f,
                1f,
                timer / fadeDuration
            );

            yield return null;
        }

        fadePanel.alpha = 1f;

        SceneManager.LoadScene(sceneName);   
    }
}