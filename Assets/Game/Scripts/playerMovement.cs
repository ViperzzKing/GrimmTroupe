using UnityEngine;
using System.Collections;

//tells the game to give object these components
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class playerMovement : MonoBehaviour
{
    public bool isFacingRight = true;
    public float moveX;

    public float jumpHeight = 20f;
    public float speed = 30f;

    private Vector3 start;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private DashMechanic dash;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 start = rb.transform.position;
        dash = FindAnyObjectByType<DashMechanic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dash.isDashing)
        {
            return;
        }

        moveX = Input.GetAxis("Horizontal");



        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //if help jumps higher
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        }

        if(Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            //If tapped jumps smaller
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (dash.isDashing)
        {
            return;
        }

        if (moveX > 0 && !isFacingRight || moveX < 0 && isFacingRight)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        //OverlapCircle checking for groundCheck gameObject on Player, size of circle, when is the ground?
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Flip()
    {
        // currentScale becomes our current size
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        // it equals the oppisate ( ! )
        isFacingRight = !isFacingRight;

    }

   
}
