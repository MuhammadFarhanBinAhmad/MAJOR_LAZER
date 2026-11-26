using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;

    void Start()
    {
        flashImage.color = new Color(1f, 0.6f, 0f, 0.7f);
        flashImage.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            StartCoroutine(FlashEffect());
        }
    }

    IEnumerator FlashEffect()
    {
        flashImage.enabled = true;
        flashImage.color = new Color(1f, 0.8f, 0f, 0.7f);

        yield return new WaitForSeconds(1f);

        flashImage.color = new Color(1f, 0.8f, 0f, 0f);
        flashImage.enabled = false;
    }
}
