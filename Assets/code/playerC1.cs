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

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //groundcheck and drag
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, 1.2f);
        if (grounded)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = drag * aerialDrag;
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) & grounded)
        {
            grounded = false;
            jump();
            playeranm.jump();
            jumping = true;
        }
        if (jumping && rb.velocity.y < 0f)
        {
            gonnaland = true;
        }
        if (jumping && grounded && gonnaland)
        {
            jumping = false;
            playeranm.land();
            gonnaland = false;
        }



        //sprint
        if (Input.GetKey(KeyCode.LeftShift) && grounded)
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
            rb.AddForce(GetSlopeMoveDir() * speed * 10f, ForceMode.Force);
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
