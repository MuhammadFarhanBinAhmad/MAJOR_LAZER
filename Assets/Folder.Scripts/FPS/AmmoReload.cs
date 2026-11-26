using TMPro;
using UnityEngine;

public class AmmoReload : MonoBehaviour
{
    public int reserveAmmo = 10;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reserveAmmoText;
    public AmmoManager ammoManager;
    // public int _ammo;

    public void ReloadAmmo()
    {
        if (ammoManager._ammo == 0)
        {
            ammoManager._ammo = ammoManager._ammo + 10;
            reserveAmmo = reserveAmmo - 10;
            ammoText.text = ammoManager._ammo.ToString();
            reserveAmmoText.text = reserveAmmo.ToString();
        }
        else
        {
            Debug.Log("You have ammo. Don't need to reload.");
            return;
        }
        
        
    }

    
}