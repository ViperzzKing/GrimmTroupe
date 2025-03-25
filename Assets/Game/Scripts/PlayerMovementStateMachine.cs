using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(TrailRenderer))]

public class PlayerMovementStateMachine : MonoBehaviour
{
    [SerializeField] private State currentState;
    GameManager gm;

    //-------------Movement--------------------\\

    [Header("Movement")]

    [SerializeField] private bool isFacingRight = true;

    private float moveHorizontal;

    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float walkingSpeed = 7f;
    [SerializeField] private float gravityUp = 10f, gravityDown = 20f;


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
    [SerializeField] private float dashGravity = 2f;

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


    //-------------------------------------------------\\


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        // Make sure our rigidbody is set up correctly
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        currentState = State.Walk;

        start = new Vector2(rb.transform.position.x, rb.transform.position.y);
    }


    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                // If Idle call IdleState Function .... so on
                IdleState();
                break;
            case State.Walk:
                WalkState();
                break;
            case State.Rise:
                RiseState();
                break;
            case State.Fall:
                FallState();
                break;
            case State.Dash:
                DashState();
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
            currentState = State.Dash;
        }

        if (rb.linearVelocity.x == 0f && rb.linearVelocity.y == 0f)
        {
            currentState = State.Idle;
        }

    }

    private void FixedUpdate()
    {
        // While Dashing = True, Keep doing only this part of fixed update, as return loops it
        if (isDashing)
        {
            return;
        }



        // Check if we are moving positive or negative and then run Flip()
        if (moveHorizontal > 0 && !isFacingRight || moveHorizontal < 0 && isFacingRight)
        {
            Flip();
        }

    }


    //-------------------State Functions--------------------------\\


    private void IdleState()
    {

        Vector2 inputMovement = GetMovementFromInput();
        rb.linearVelocity = inputMovement;

        if (rb.linearVelocity.x != 0f || rb.linearVelocity.y != 0f)
        {
            currentState = State.Walk;
        }

        if (!IsGrounded())
        {
            currentState = State.Fall;
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                RiseAtSpeed(jumpPower);
                gravityDown = 20f;
            }
        }

    }
    private void WalkState()
    {


        // Get our input direction
        Vector2 inputMovement = GetMovementFromInput();

        inputMovement.y = Mathf.Clamp(rb.linearVelocity.y - gravityDown * Time.deltaTime, 0f, float.PositiveInfinity);
        rb.linearVelocity = inputMovement;


        if (!IsGrounded())
        {
            currentState = State.Fall;
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                RiseAtSpeed(jumpPower);
                gravityDown = 20f;
            }
        }

    }
    private void RiseState()
    {
        currentState = State.Rise;

        Vector2 inputMovement = GetMovementFromInput();

        // We are rising so use upward gravity
        inputMovement.y = rb.linearVelocity.y - gravityUp * Time.deltaTime;
        rb.linearVelocity = inputMovement;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.2f, rb.linearVelocity.y);

        // If linear velove.y is less than 0, we started to fall
        if (rb.linearVelocity.y < 0f)
        {
            currentState = State.Fall;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            currentState = State.Fall;
            gravityDown = 40f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

    }
    private void FallState()
    {


        Vector2 inputMovement = GetMovementFromInput();

        // We are falling so use down gravity
        inputMovement.y = rb.linearVelocity.y - gravityDown * Time.deltaTime;
        rb.linearVelocity = inputMovement;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.2f, rb.linearVelocity.y);


        if (IsGrounded())
        {
            currentState = State.Walk;
        }

    }
    private void DashState()
    {

        if (!isDashing)
        {
            currentState = State.Fall;
        }

    }


    //------------------Enumeators--------------------------\\


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = dashGravity;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        tr.emitting = false;
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


    private Vector2 GetMovementFromInput()
    {

        // Get Horizontal keys
        moveHorizontal = Input.GetAxis("Horizontal");

        // Put it into movement
        Vector2 moveDirection = new Vector2(moveHorizontal * walkingSpeed, rb.linearVelocity.y);

        return moveDirection;

    }

    public void RiseAtSpeed(float speed)
    {
        // Give rigidbody upward speed
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);

        currentState = State.Rise;
    }

    private bool IsGrounded()
    {
        //OverlapCircle checking for groundCheck gameObject on Player, size of circle, when is the ground?
        return Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer);
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

    private void Restart()
    {
        transform.position = start;
    }
}
