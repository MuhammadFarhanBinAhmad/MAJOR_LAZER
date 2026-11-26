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
            // ammoManager._ammo = ammoManager._ammo + 10;
            // reserveAmmo = reserveAmmo - 10;
            // ammoText.text = ammoManager._ammo.ToString();
            // reserveAmmoText.text = reserveAmmo.ToString();
            StartCoroutine(ReloadCoroutine());  
            Debug.Log("Reloading ammo...");     
        }
        else
        {
            Debug.Log("You have ammo. Don't need to reload.");
            return;
        }
        
        
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        ammoManager._ammo += _ammoToReload; // Add the specified amount of ammo to the current ammo
        reserveAmmo -= _ammoToReload; // Subtract the specified amount from the reserve ammo
        ammoText.text = ammoManager._ammo.ToString(); // Update the ammo text
        reserveAmmoText.text = reserveAmmo.ToString(); // Update the reserve ammo text
        
    }


    
}