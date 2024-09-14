using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    bool elevatnt;
    bool elevating;
    bool elevated;
    bool door;

    public switchscript switchA;
    public switchscript switchB;
    public bool switchBenable;
    public switchscript switchC;
    public bool switchCenable;
    public switchscript switchD;
    public bool switchDenable;

    public Collider doorColider;
    public Renderer doorRenderer;

    public Transform player;
    public Transform detector;
    public float activationdistance;
    public float finalHeight;
    public float maxspeed;

    public bool savepoint;
    public float savepointpos;
    float savefilepoint;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        door = false;
        if (switchB == null)
        {
            switchB = switchA;
        }
        if (switchC == null)
        {
            switchC = switchA;
        }
        if (switchD == null)
        {
            switchD = switchA;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (switchA.switched == true && switchBenable == false && switchCenable == false && switchDenable == false)
        {
            elevatnt = true;
        }
        if (switchA.switched == true && switchB.switched == true && switchBenable == true && switchCenable == false && switchDenable == false)
        {
            elevatnt = true;
        }
        if (switchA.switched == true && switchB.switched == true && switchC.switched == true && switchBenable == true && switchCenable == true && switchDenable == false)
        {
            elevatnt = true;
        }
        if (switchA.switched == true && switchB.switched == true && switchC.switched == true && switchD.switched == true && switchBenable == true && switchCenable == true && switchDenable == true)
        {
            elevatnt = true;
        }


        if(elevatnt == true && elevating == false && elevated == false)
        {
            door = true;
            if(Vector3.Distance(player.position, detector.position) < activationdistance)
            {
                elevating = true;
                door = false;
            }
        }


        if(elevated == true)
        {
            door = true;
        }



        if (door)
        {
            doorColider.enabled = false;
            doorRenderer.enabled = false;
        }
        else
        {
            doorColider.enabled = true;
            doorRenderer.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (elevating == true && elevated == false)
        {
            if (transform.position.y < finalHeight - 1f)
            {
                speed += 0.005f;
            }
            else
            {
                speed -= 0.0001f;
            }
            if (transform.position.y >= finalHeight)
            {
                speed = 0f;
                elevated = true;
            }
            speed = Mathf.Clamp(speed, 0f, maxspeed);
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);

        }
    }
}
