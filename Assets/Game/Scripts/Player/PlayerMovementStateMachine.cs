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
    }

    private void FixedUpdate()
    {
        // While Dashing = True, Keep doing only this part of fixed update, as return loops it
        if (isDashing)
        {
            return;
        }

        Vector2 inputMovement = GetMovementFromInput();

        // Check if we are moving positive or negative and then run Flip()
        if (inputMovement.x < 0 && !isFacingRight || inputMovement.x > 0 && isFacingRight)
        {
            Flip();
        }

    }


    //-------------------State Functions--------------------------\\


    private void IdleState()
    {

        Vector2 inputMovement = GetMovementFromInput();
        inputMovement *= walkingSpeed;

        rb.linearVelocity = inputMovement;

        if (!IsGrounded())
        {
            // We should be falling
            currentState = State.Fall;
        }
        else
        {
            // If we press jump
            if (Input.GetButton("Jump"))
            {
                // go to our rise state
                RiseAtSpeed(jumpPower);
            }
        }

        // Make our linearVelocity into varibles so it can be used to check for it
        float xVelocity = rb.linearVelocity.x;
        float yVelocity = rb.linearVelocity.y;

        // If one of these isnt 0 change our state
        if (xVelocity != 0f || yVelocity != 0f)
        {
            currentState = State.Walk;
        }


    }
    private void WalkState()
    {

        // Get our direction based on what were were looking
        Vector2 inputMovement = GetMovementFromInput();
        inputMovement *= walkingSpeed;
        // ^ ^ ^ Increase that using our walk speed

        // Make sure were on the ground, but not building up vertical speed                        // no upper limit
        inputMovement.y = Mathf.Clamp(rb.linearVelocity.y - gravityDown * Time.deltaTime, 0f, float.PositiveInfinity);

        // Apply movement to rigidbody
        rb.linearVelocity = inputMovement;

        // Check if were supposed to be in different state
        if (!IsGrounded())
        {
            // We should be falling
            currentState = State.Fall;
        }
        else
        {
            // If we press jump
            if (Input.GetButton("Jump"))
            {
                // go to our rise state
                RiseAtSpeed(jumpPower);
            }
        }

        // Make our linearVelocity into varibles so it can be used to check for it
        float xVelocity = rb.linearVelocity.x;
        float yVelocity = rb.linearVelocity.y;

        // If both of these are 0 become idle
        if(xVelocity == 0f && yVelocity == 0f)
        {
            currentState = State.Idle;
        }

    }
    private void RiseState()
    {

        Vector2 inputMovement = GetMovementFromInput();
        inputMovement *= walkingSpeed;

        inputMovement.y = rb.linearVelocity.y - gravityUp * Time.deltaTime;
        // We are rising, so use upward gravity

        rb.linearVelocity = inputMovement;

        // If linearVelocity.y is less than 0, were falling so go to Fall State
        if(rb.linearVelocity.y < 0f)
        {
            currentState = State.Fall;
        }


    }
    private void FallState()
    {
        // Same as Rise state gravity but use downward gravity instead
        Vector2 inputMovement = GetMovementFromInput();
        inputMovement *= walkingSpeed;

        inputMovement.y = rb.linearVelocity.y - gravityDown * Time.deltaTime;

        rb.linearVelocity = inputMovement;

        // After falling when we land on ground change state to Walking
        if (IsGrounded())
        {
            currentState = State.Walk;
        }


    }
    private void DashState()
    {

        // Maintain dash velocity during the dash state
        if (isDashing)
        {
            rb.gravityScale = dashGravity;

            Vector2 dashVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            rb.linearVelocity = dashVelocity;



        }
        else
        {
            currentState = State.Fall;
            rb.gravityScale = 1f;
        }
    }


    //------------------Enumeators--------------------------\\


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.gravityScale = dashGravity;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
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


    private void RiseAtSpeed(float speed)
    {

        // Give our rigidbody upward speed
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);

        currentState = State.Rise;

    }

    private Vector2 GetMovementFromInput()
    {
        Vector2 inputThisFrame = new Vector2();
        inputThisFrame.x = Input.GetAxis("Horizontal");
        inputThisFrame.y = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(inputThisFrame.x * walkingSpeed, inputThisFrame.y);

        return moveDirection;
    }

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

    private void Restart()
    {
        transform.position = start;
    }
}
