using UnityEngine;

public class ADS : MonoBehaviour
{
    public GameObject weapon;
    public GameObject crosshair;

    void Start()
    {
        crosshair.SetActive(false);
    
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            weapon.SetActive(false);
            crosshair.SetActive(true);
        }
        else
        {
            weapon.SetActive(true);
            crosshair.SetActive(false);
        }
    }
}
