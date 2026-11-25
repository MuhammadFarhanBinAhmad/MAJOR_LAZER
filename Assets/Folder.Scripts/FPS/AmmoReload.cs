//using TMPro;
using UnityEngine;

public class AmmoReload : MonoBehaviour
{
    public int maxAmmo = 10;
    //public TextMeshProUGUI ammoText;
    public int _ammo;

    public void ReloadAmmo()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_ammo == 0)
            {
                _ammo = _ammo + 10;
                maxAmmo = maxAmmo - 10;
                //ammoText.text = _ammo.ToString();
            }
            else
            {
                //Debug.Log("Ammo is not empty. Cannot reload.");
            }
        }
        
    }

    
}