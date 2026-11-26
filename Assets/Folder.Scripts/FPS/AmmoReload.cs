using TMPro;
using System.Collections;
using UnityEngine;

public class AmmoReload : MonoBehaviour
{
    public int reserveAmmo = 10;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reserveAmmoText;
    public AmmoManager ammoManager;
    public int _ammoToReload = 1; // Amount of ammo to reload

    public void ReloadAmmo()
    {
        if (ammoManager._ammo == 0)
        {
            ammoManager._ammo = ammoManager._ammo + _ammoToReload;
            reserveAmmo = reserveAmmo - _ammoToReload;
            ammoText.text = ammoManager._ammo.ToString();
            reserveAmmoText.text = reserveAmmo.ToString();
            Debug.Log("Reloading ammo...");
            
        }
        else
        {
            Debug.Log("You have ammo. Don't need to reload.");
            return;
        }
        
        
    }


    
}