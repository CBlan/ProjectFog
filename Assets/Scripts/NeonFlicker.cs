using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonFlicker : MonoBehaviour {

    Renderer rend;
    Material mat;
    private float maxIntensity = 3.5f;
    private float minIntensity = 2.5f;
    public float flickerTime = 0.1f;

    // Use this for initialization
    void Start () {
        rend= GetComponent<Renderer>();
        mat = rend.material;
        //print(mat.GetColor("_Color"));
        StartCoroutine(Flicker());
    }
	
    IEnumerator Flicker()
    {
        while (true)
        {
            mat.SetColor("_EmissionColor", mat.GetColor("_Color") * Random.Range(minIntensity, maxIntensity));
            yield return new WaitForSeconds(flickerTime);

        }
    }
}
