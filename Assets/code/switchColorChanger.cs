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
        color = -0.75f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (switchscript.switched == true && color < 1f)
        {
            color += 0.02f;
        }
        if (switchscript.switched == false && color > -0.75f)
        {
            color -= 0.02f;
        }
        mat.SetColor("_EmissionColor", new Color((Mathf.Clamp(1 - color * 2, 0f, 1f)) * intensity, Mathf.Clamp(color, 0f, 1f) * intensity, 0f, 0f));
    }
}
