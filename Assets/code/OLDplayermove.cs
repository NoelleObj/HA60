using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public float speed;
    public float jumpforce;
    public float downjump;
    public float drag;

    public float maxSlopeAngle;
    private RaycastHit slopehit;

    float multiplier;
    float camY;
    float counter;
    float sinMult;
    float lastVel;
    float lastlastvel;
    float landOffset;
    float landSpeed;
    float Lcounter;
    bool landing;

    public Transform orientation;
    public Rigidbody rb;


    float hInput;
    float vInput;

    bool grounded;

    Vector3 moveDirection;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveinput();
        lastlastvel = lastVel;
        lastVel = rb.velocity.y;
    }

    private void FixedUpdate()
    {
        Move();
        
    }

    private void moveinput()
    {



        //groundcheck
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, 1.2f);
        if (grounded)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0;
        }


        if(Input.GetKeyDown(KeyCode.Space) & grounded)
        {
            jump();
        }

        //sprint
        if(Input.GetKey(KeyCode.LeftShift) & grounded)
        {
            multiplier = 1.6f;
        }
        else
        {
            multiplier = 1f;
        }

        if (!grounded)
        {
            multiplier = 0.1f;
        }
        

    }

    private void Move()
    {
        if (onslope())
        {
            rb.AddForce(GetSlopeMoveDir() * speed * 10f, ForceMode.Force);
        }
        moveDirection = orientation.forward * vInput + orientation.right * hInput;
        rb.AddForce(moveDirection * speed * 10f * multiplier, ForceMode.Force);
    }

    private void jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
    }

    private bool onslope()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, out slopehit, 1.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopehit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopehit.normal).normalized;
    }
}
