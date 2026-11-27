
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadMusic : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip newMusic;
    public string targetScene;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetScene)
        {
            audioSource.clip = newMusic;
            audioSource.Play();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
