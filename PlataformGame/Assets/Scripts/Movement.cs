using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D rb;
    private Transform transform;
    private SpriteRenderer sprite;
    private RigidbodyConstraints2D constraints;
    private Animator animator;

    [Header("Movement")]
    public float jumpSpeed;
    public float walkSpeed;
    public float dashSpeed;

    private bool dashed = false;
    private bool jump = false;
    private bool walk = false;
    private bool grabed = false;
    private bool facing = true;
    private bool spriteJump = false;

    private float x = 0f;
    private float y = 0f;

    private bool canDash = true;

    [Header("Collision")]
    private bool isGrounded = false;
    private bool isWall = false;
    public float groundLength = 1f;
    public float wallLength = 1f;
    
    public Vector3 colliderGroundL;
    public Vector3 colliderGroundR;
    public Vector3 colliderWallL;
    public Vector3 colliderWallR;
    public LayerMask Ground;


    bool enable = true;
    Coroutine dashCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        constraints = rb.constraints;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position + colliderGroundL, Vector2.down, groundLength, Ground) || Physics2D.Raycast(transform.position - colliderGroundR, Vector2.down, groundLength, Ground);
        isWall = Physics2D.Raycast(transform.position + colliderWallL, Vector2.down, wallLength, Ground) || Physics2D.Raycast(transform.position - colliderWallR, Vector2.up, wallLength, Ground);

        
        if (isWall)
        {
            StopCoroutine(dashCoroutine);
            animator.SetBool("Dash", false);
            enable = true;
        }

        if (enable)
        {

            if (Input.GetButton("Horizontal") && !grabed)
            {
                walk = true;
                
            }

            if (facing && Input.GetAxis("Horizontal") < 0)
            {
                sprite.flipX = !sprite.flipX;
                facing = false;
            }
            if (!facing && Input.GetAxis("Horizontal") > 0)
            {
                sprite.flipX = !sprite.flipX;
                facing = true;
            }

            if (rb.velocity.y < 0f && !isGrounded)
            {
                animator.SetBool("Jump", true);

            }
            if (rb.velocity.y >= 0)
            {
                animator.SetBool("Jump", false);

            }

            if (Input.GetButtonDown("Jump") && (isGrounded || isWall))
            {
                jump = true;
                grabed = false;
            }

            if (Input.GetMouseButtonDown(0) && canDash)
            {

                dashed = true;
                canDash = false;
            }

            if (isGrounded)
            {
                canDash = true;
            }


            if (isWall && !isGrounded && !grabed)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    walk = false;
                    grabed = true;

                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                grabed = false;

            }
            

        }
    }
    void FixedUpdate()
    {
        if (enable)
        {

            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            
            if (walk)
            {
                Walking();
                if (!animator.GetBool("Jump"))
                {
                    animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
                    
                }
                else
                {
                    animator.SetFloat("Speed", 0);
                }

            }

            if (jump && (isGrounded || (isWall && grabed)))
            {
                Jump();
                jump = false;

            }

            if (dashed)
            {

                animator.SetBool("Dash", true);
                Dash();

                grabed = false;
                dashed = false;
                
            }

            if (grabed)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.SetBool("Grab", true);

            }
            if (!grabed)
            {
                rb.constraints = constraints;
                animator.SetBool("Grab", false);

            }


        }


    }


    private void Walking()
    {
        //transform.Translate(x * Time.deltaTime * walkSpeed, 0, 0);
        rb.velocity += Vector2.right * x * walkSpeed;
    }

    private void Jump()
    {     
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpSpeed;
    }


    private void Dash()
    {
        enable = false;
        Vector2 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = new Vector3(pz.x - transform.position.x, pz.y - transform.position.y).normalized;
        
        dashCoroutine =  StartCoroutine(DashWait(transform.position, transform.position + dir * dashSpeed, 50));
        

    }

    IEnumerator DashWait( Vector3 a, Vector3 b, float speed)
    {

        enable = false;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        
        while (t <= 1.0f)
        {

            t += step;

            transform.position = Vector3.Lerp(a, b, t);
            
            yield return new WaitForFixedUpdate();        
        }

        animator.SetBool("Dash", false);
        enable = true;
        //transform.position = b;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderGroundL, transform.position + colliderGroundL + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderGroundR, transform.position - colliderGroundR + Vector3.down * groundLength);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + colliderWallL, transform.position + colliderWallL + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderWallR, transform.position - colliderWallR + Vector3.down * groundLength);
    }
    
}
