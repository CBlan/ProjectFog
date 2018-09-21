using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePulse : MonoBehaviour {

    Renderer rend;
    Material mat;
    public float maxIntensity = 3f;
    public float pulseTime = 6f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    private void Update()
    {
        mat.SetColor("_EmissionColor", mat.GetColor("_Color") * Mathf.PingPong(Time.time * pulseTime, maxIntensity));
    }

}
