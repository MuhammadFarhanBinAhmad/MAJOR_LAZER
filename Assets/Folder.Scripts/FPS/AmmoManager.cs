using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public int _ammo = 10;
    public TextMeshProUGUI _text;

    public void DecreaseAmmo()
    {
        if (_ammo > 0)
        {
            _ammo--;
            _text.text = _ammo.ToString();
        }
        
    }
    
}
