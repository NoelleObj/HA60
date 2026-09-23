using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerC1 : MonoBehaviour
{
    public Transform orientation;
    public Transform camfollow;
    public Rigidbody rb;
    public playerAnimation playeranm;

    public float followoffset;
    public float speed;
    public float jumpforce;
    public float drag;

    public float aerialDrag;
    public float aerialSpeed;

    public float maxSlopeAngle;
    private RaycastHit slopehit;

    float vInput;
    float hInput;

    bool grounded;

    Vector3 moveDirection;

    float multiplier;
    bool jumping;
    bool gonnaland;
    bool truejump;

    // Start is called before the first frame update
    void Start()
    {
      grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //groundcheck and drag
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, 1.2f);
        if (grounded)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.linearDamping = drag * aerialDrag;
        }

        //jump
        if (Input.GetButtonDown("jump") & grounded)
        {
            grounded = false;
            jump();
            playeranm.jump();
            jumping = true;
            truejump = true;
        }
        if (jumping && rb.linearVelocity.y < 0f)
        {
            gonnaland = true;
        }
        if (jumping && grounded && gonnaland)
        {
            jumping = false;
            playeranm.land();
            gonnaland = false;
            truejump = false;
        }



        //sprint
        if (Input.GetButton("run") && grounded)
        {
            multiplier = 1.6f;
        }
        else
        {
            multiplier = 1f;
        }
        //jumpspeed
        if(!grounded)
        {
            multiplier = aerialSpeed;
        }

        //punch
        if (Input.GetButtonDown("punch"))
        {
            playeranm.punch();

        }
        if(rb.linearVelocity.y < 0f && grounded == false && jumping == false && truejump == false){
            playeranm.jumping();
            jumping = true;
            gonnaland = true;
        }
    }
    private void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {

        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");
        if (onslope())
        {
            rb.AddForce(GetSlopeMoveDir() * speed * 5f, ForceMode.Force);
        }
        moveDirection = orientation.forward * vInput + orientation.right * hInput;
        rb.AddForce(moveDirection.normalized * speed * 10f * multiplier, ForceMode.Force);
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
