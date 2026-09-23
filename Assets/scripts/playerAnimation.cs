using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anm.SetFloat("speedpercent", rb.linearVelocity.magnitude / 15f);
        print(rb.linearVelocity.magnitude);
       
    }
    public void jump()
    {
        anm.SetTrigger("jump");
    }
    public void jumping()
    {
        anm.SetTrigger("jumping");
    }

    public void land()
    {
        anm.SetTrigger("land");
    }
    public void switche()
    {
        anm.SetTrigger("switch");
    }
    public void upswitch()
    {
        anm.SetTrigger("upswitch");
    }
    public void punch()
    {
        anm.SetTrigger("punch");
    }
}
