using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public switchscript switchA;
    public switchscript switchB;
    public bool switchBenable;
    public switchscript switchC;
    public bool switchCenable;
    public switchscript switchD;
    public bool switchDenable;

    public bool open;

    public Collider colider;
    public Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(switchA.switched == true && switchBenable == false && switchCenable == false && switchDenable == false)
        {
            StartCoroutine(wait());
        }
        if (switchA.switched == true && switchB.switched == true && switchBenable == true && switchCenable == false && switchDenable == false)
        {
            StartCoroutine(wait());
        }
        if (switchA.switched == true && switchB.switched == true && switchC.switched == true && switchBenable == true && switchCenable == true && switchDenable == false)
        {
            StartCoroutine(wait());
        }
        if (switchA.switched == true && switchB.switched == true && switchC.switched == true && switchD.switched == true && switchBenable == true && switchCenable == true && switchDenable == true)
        {
            StartCoroutine(wait());
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);

        colider.enabled = false;
        renderer.enabled = false;
        open = true;
    }
}
