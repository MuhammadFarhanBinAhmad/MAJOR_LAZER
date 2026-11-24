using UnityEngine;

public class Destroy : MonoBehaviour
{
    public ScoreManager scoreManager;
    public bool isTargetHit;
    public void Update()
    {
        if (isTargetHit == true)
        {
            scoreManager.AddScore();
        }
    }

    
}
