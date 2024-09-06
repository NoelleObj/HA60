using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchColorChanger : MonoBehaviour
{
    public Material mat;
    public float intensity;
    float color;
    public switchscript switchscript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (switchscript.switched == true && color < 1f)
        {
            color += 0.01f;
        }
        if (switchscript.switched == false && color > 0f)
        {
            color -= 0.05f;
        }
        mat.SetColor("_EmissionColor", new Color((1 - color) * intensity, Mathf.Clamp(color * 2f - 1f, 0f, 1f) * intensity, 0f, 0f));
    }
}
