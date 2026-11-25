//using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public int _ammo;
    //public TextMeshProUGUI _text;
    
    public void DecreaseAmmo()
    {
        if (_ammo > 0)
        {
            _ammo--;
            //_text.text = _ammo.ToString();
        }
        else
        {
            //Debug.Log("Out of ammo!");
        }
    }
}
