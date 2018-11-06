using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaskScroll : MonoBehaviour {

    private Image image;
    private float x = 0;

    public float scrollSpeed = 0.01f;
    public Color imageColor;

    // Use this for initialization
    private void OnEnable()
    {
        image = GetComponent<Image>();
        image.material.SetTextureOffset("_Mask", new Vector2(1, 0));
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            x += scrollSpeed;
            if (x > 1)
            {
                x = 0;
            }
            image.material.SetTextureOffset("_Mask", new Vector2(x, 0));
            image.material.SetColor("_Color", imageColor);
            //yield return null;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

}
