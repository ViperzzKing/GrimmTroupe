using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovementStateMachine : MonoBehaviour
{
    [SerializeField] private State currentState;

    //-------------Movement--------------------\\

    [Header("Movement")]

    [SerializeField] private bool isFacingRight;
    [SerializeField] private bool jumpDashTech = true;

    private float moveHorizontal;

    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float walkingSpeed = 10f;
    [SerializeField] private float gravityUp, gravityDown;

    Vector3 start;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    //-----------Dashing------------------------\\

    [Header("Dashing")]

    [SerializeField] private bool dashCollected = true;

    private bool canDash = true;
    public bool isDashing;

    [SerializeField] private float dashingPower = 30f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = .5f;
    [SerializeField] private float dashGravity = 1f;

    public float noDamageTime = 0.5f;
    public bool noDamage = false;

    [SerializeField] TrailRenderer tr;


    //----------States-----------------------------\\

        private enum State
    {
        Idle,
        Walk,
        Rise,
        Fall,
        Dash
    }

    private bool changeState_Idle = false;
    private bool changeState_Walk = false;
    private bool changeState_Fall = false;
    private bool changeState_Rise = false;


    //-------------------------------------------------\\

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        currentState = State.Idle;

        start = new Vector2(rb.transform.position.x, rb.transform.position.y);
    }

    
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Walk:
                break;
            case State.Rise:
                break;
            case State.Fall:
                break;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCollected)
        {
            StartCoroutine(Dash());
            StartCoroutine(NoDamage());
        }

        moveHorizontal = Input.GetAxis("Horizontal");

        Jump();
    }

    private void FixedUpdate()
    {
        // While Dashing = True, Keep doing only this part of fixed update, as return loops it
        if (isDashing)
        {
            return;
        }

        // Check if we are moving positive or negative and then run Flip()
        if (moveHorizontal < 0 && !isFacingRight || moveHorizontal > 0 && isFacingRight)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(moveHorizontal * walkingSpeed, rb.linearVelocity.y);

    }

    //------------------------------------------------------\\

    private void Idle()
    {
        Debug.Log("Idle State");
        // Check if state changed
        if (changeState_Idle)
        {
            currentState = State.Walk;
        }
    }
    private void Walk()
    {
        Debug.Log("Walk State");
        // Check if change state
        if (changeState_Walk)
        {
            currentState = State.Rise;
        }
    }
    private void Rise()
    {
        Debug.Log("Rise State");
        // Check if change state
        if (changeState_Rise)
        {
            currentState = State.Fall;
        }
    }
    private void Fall()
    {
        Debug.Log("Fall State");
        // Check if change state
        if (changeState_Fall)
        {
            currentState = State.Idle;
        }
    }

    //--------------------------------------------\\

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orginalGravity = rb.gravityScale;
        rb.gravityScale = dashGravity;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator NoDamage()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        noDamage = true;
        yield return new WaitForSeconds(noDamageTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        noDamage = false;
    }

    //---------------------------------------------------------------\\

    private bool IsGrounded()
    {
        //OverlapCircle checking for groundCheck gameObject on Player, size of circle, when is the ground?
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        // currentScale becomes our current size
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        // it equals the oppisate ( ! )
        isFacingRight = !isFacingRight;
    }

    private void Jump()
    {
        if (!jumpDashTech)
        {
            if (isDashing)
            {
                return;
            }
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //if help jumps higher
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            //If tapped jumps smaller
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void Restart()
    {
        transform.position = start;
    }
}
