using UnityEngine;

public class ReloadAnim : MonoBehaviour
{
    public Animator _anim;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            _anim.SetTrigger("Reload");
        }
    }
}
