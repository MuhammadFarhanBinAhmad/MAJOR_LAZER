using UnityEngine;

public class ExitMenuButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void QuitGame()
    {
       Debug.Log("Game is exiting..."); 
           Application.Quit();

    }
}