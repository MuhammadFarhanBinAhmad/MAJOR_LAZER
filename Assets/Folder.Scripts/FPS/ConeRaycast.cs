using UnityEngine;

public class ConeRaycast : MonoBehaviour
{
    public float rayDistance = 20f;
    public float coneAngle = 30f;
    public int rayCount = 100;
    public AmmoManager ammoManager;
    public int _ammo = 10;
    public AmmoReload ammoReload; // Reference to the AmmoReload script
    public ScoreManager scoreManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoManager._ammo > 0)
            {
                ShootCone();
                ammoManager.DecreaseAmmo();
            }
            else
            {
                Debug.Log("Out of ammo! Reloading...");
                ammoReload.ReloadAmmo(); // Call the ReloadAmmo method from the AmmoReload script
                return;
            }
            
        }
    }

    void ShootCone()
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = RandomDirectionInCone();

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, rayDistance))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.red, 2f);
                if (hit.collider.gameObject.CompareTag("Bird1"))
                {
                    scoreManager.AddScore();
                    Destroy(hit.collider.gameObject);
                    return; 
                }
                if (hit.collider.gameObject.CompareTag("Bird2"))
                {
                    scoreManager.AddScore();
                    scoreManager.AddScore();
                    Destroy(hit.collider.gameObject);
                    return; 
                }
                if (hit.collider.gameObject.CompareTag("Bird3"))
                {
                    scoreManager.AddScore();
                    scoreManager.AddScore();
                    scoreManager.AddScore();
                    Destroy(hit.collider.gameObject);
                    return; 
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayDistance, Color.green, 2f);
            }
            
        }
    }
    Vector3 RandomDirectionInCone()
    {
    float angle = coneAngle * Mathf.Deg2Rad;
    
    float randomAngle = Random.Range(0f, 2f * Mathf.PI);
    float randomRadius = Random.Range(0f, angle);
    
    Vector3 localDirection = new Vector3(Mathf.Sin(randomRadius) * Mathf.Cos(randomAngle), Mathf.Sin(randomRadius) * Mathf.Sin(randomAngle), Mathf.Cos(randomRadius));
    return transform.TransformDirection(localDirection);
    }
}
