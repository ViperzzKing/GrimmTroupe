using UnityEngine;

//tells the game to give object these components
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class playerMovement : MonoBehaviour
{
    private bool isFacingRight = true;


    private float MoveX;

    private Vector3 start;

    public Rigidbody2D rb;

    [Tooltip("Changes the speed of moving left & right")]
    public float speed = 10;

    [Tooltip("Changes the distance the player jumps")]
    public float jumpHeight = 10;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        start = new Vector2(rb.transform.position.x, rb.position.y + 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(MoveX * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpHeight * 100));
        }
    }

    private void Flip()
    {
        if(isFacingRight && MoveX < 0f || isFacingRight && MoveX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= 1f;
            transform.localScale = localScale;
        }
    }

}
