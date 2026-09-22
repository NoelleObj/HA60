using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDplayerC0 : MonoBehaviour
{
    public Transform player;
    public Transform orientation;
    public Transform playermodel;
    public Transform rotationthingy;
    public Rigidbody rb;
   
    public float rotspeed;

    Vector3 oldforward;

    void Start()
    {
        oldforward = new Vector3(0f, 0f, 1f);
    }

    void Update()
    {
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        //rotate oriobject
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 inputDir = player.forward * vInput + player.right * hInput;

        //cm backfix
        //orientation.forward = viewDir.normalized;
        if (Input.GetAxis("Vertical") < 0f && Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Mouse X") == 0f)
        {
            print(orientation.forward);
            orientation.forward = oldforward;
        }
        else
        {
            orientation.forward = viewDir.normalized;
        }


        if (inputDir != Vector3.zero)
        {
            Vector3 lerpDir = Vector3.Slerp(rotationthingy.forward, inputDir.normalized, Time.deltaTime * rotspeed);
            rotationthingy.forward = lerpDir;
            playermodel.rotation = Quaternion.Euler(0f, orientation.eulerAngles.y + rotationthingy.eulerAngles.y, 0f);
        }

        oldforward = orientation.forward;
    }
}
