using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaskScroll : MonoBehaviour {

    private Image image;
    private float x = 0;
    public float scrollSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        image.material.SetTextureOffset("_Mask", new Vector2(1,0));
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
            yield return null;
        }
    }
}
