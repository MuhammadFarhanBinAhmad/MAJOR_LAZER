using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{ 

    public string sceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}

