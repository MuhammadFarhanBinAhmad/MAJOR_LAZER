using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{ 

    public string sceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}

