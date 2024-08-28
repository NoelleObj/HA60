using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerC0 : MonoBehaviour
{
    public Transform player;
    public Transform orientation;
    public Transform playermodel;
    public Transform followtarget;
    public Transform Camerabase;
    public Transform Camerayaw;
    public Transform Cameratilt;
    public Transform Cameraholder;
    public Transform Ray;

    public float rotspeed;
    public float followspeed;
    public float sensX;
    public float sensY;
    public float minrotY;
    public float maxrotY;
    float camtilted;

    public float colisionradius1;
    public float camdist;

    Vector3 followvelocity = Vector3.zero;
    Vector3 targetposition;
    public LayerMask colisionlayer;


    void Start()
    {

    }

    void Update()
    {
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        //rotate oriobject
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        orientation.forward = viewDir.normalized;

        //rotate model
        if (inputDir != Vector3.zero)
        {
            playermodel.forward = Vector3.Slerp(playermodel.forward, inputDir.normalized, Time.deltaTime * rotspeed);
        }
        //follow
        targetposition = Vector3.SmoothDamp(Camerabase.position, followtarget.position, ref followvelocity, followspeed);
        Camerabase.position = targetposition;


        //rotate cam
        Camerayaw.Rotate(0f, Input.GetAxis("Mouse X") * sensX, 0f);
        camtilted += Input.GetAxis("Mouse Y") * sensY * -1;
        camtilted = Mathf.Clamp(camtilted, minrotY, maxrotY);
        Cameratilt.localRotation = Quaternion.Euler(camtilted, 0f, 0f);


        //cam collision
        RaycastHit hit;
        Physics.Raycast(Ray.position, Ray.forward, out hit, colisionradius1, colisionlayer);

        if(hit.distance != 0f && hit.distance > camdist)
        {
            Cameraholder.localPosition = new Vector3(0f, 0f, Mathf.Clamp(hit.distance*-1, camdist, 0f));
        }
        else
        {
            Cameraholder.localPosition = new Vector3(0f, 0f, camdist);
        }
    }

}
