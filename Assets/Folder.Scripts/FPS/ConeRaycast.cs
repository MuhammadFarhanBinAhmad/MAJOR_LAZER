using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeRaycast : MonoBehaviour
{
    public float rayDistance = 20f;
    public int rayCount = 100;
    
    //public float coneAngle = 30f;
    public float horizontalAngle = 40f;
    public float verticalAngle = 65f;
    
    public AmmoManager ammoManager;
    public int _ammo = 1;
    public AmmoReload ammoReload; // Reference to the AmmoReload script
    public float _reloadDuration;
    public int reserveAmmo = 90;// Reserve ammo variable
    public ScoreManager scoreManager;
    public bool shotFired = false;
    // public float _fireRate;
    // float _nxtTimeFire;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shotFired == false)
        {
            
            if (ammoManager._ammo > 0)
            {
                ShootCone();
                ammoManager.DecreaseAmmo();
                // shotFired = true;
                // StartCoroutine(delayBetweenShots());
            }
            else
            {
                Debug.Log("Out of ammo! Reload!");
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadDelay()); // Start the reload delay coroutine
            
            
        }
        
    }   

    void ShootCone()
    {
        HashSet<GameObject> hitBirds = new HashSet<GameObject>();

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = EvenlySpacedDirection(i);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, rayDistance))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.red, 2f);
                GameObject hitObject = hit.collider.gameObject;
                if (hitBirds.Add(hitObject))
                {
                    if (hitObject.CompareTag("Bird1"))
                    {
                        scoreManager.AddScore();
                        Destroy(hitObject);
                    }
                    else if (hitObject.CompareTag("Bird2"))
                    {
                        scoreManager.AddScore();
                        scoreManager.AddScore();
                        Destroy(hitObject);
                    }
                    else if (hitObject.CompareTag("Bird3"))
                    {
                        scoreManager.AddScore();
                        scoreManager.AddScore();
                        scoreManager.AddScore();
                        Destroy(hitObject);
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayDistance, Color.green, 2f);
            }
        }   
    }

    // Vector3 EvenlySpacedDirection(int index)
    // {
    //     float angle = coneAngle * Mathf.Deg2Rad;
    //     float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));
    //     float t = (index + 0.5f) / rayCount;
    //     float cosTheta = Mathf.Lerp(1f, Mathf.Cos(angle), t);
    //     float sinTheta = Mathf.Sqrt(1f - cosTheta * cosTheta);
    //     float phi = index * goldenAngle;
    //     Vector3 localDirection = new Vector3(sinTheta * Mathf.Cos(phi),sinTheta * Mathf.Sin(phi),cosTheta);
    //     return transform.TransformDirection(localDirection);
    // }

    Vector3 EvenlySpacedDirection(int index)
    {
        int columns = Mathf.CeilToInt(Mathf.Sqrt(rayCount));
        int rows = Mathf.CeilToInt((float)rayCount / columns);
        int row = index / columns;
        int column = index % columns;
        float horizontalStep = (float)column / (columns - 1);
        float verticalStep = (float)row / (rows - 1);
        float x = Mathf.Lerp(-horizontalAngle / 2f,horizontalAngle / 2f,horizontalStep);
        float y = Mathf.Lerp(-verticalAngle / 2f, verticalAngle / 2f,verticalStep);
        Vector3 localDirection = Quaternion.Euler(y, x, 0f) * Vector3.forward;
        return transform.TransformDirection(localDirection);
    }
    IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(_reloadDuration); // Adjust the delay time as needed
        ammoReload.ReloadAmmo(); // Call the ReloadAmmo method from the AmmoReload script

    }
}

