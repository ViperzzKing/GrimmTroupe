using UnityEngine;

//tells the game to give object these components
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class playerMovement : MonoBehaviour
{

    private float MoveX;

    public Rigidbody2D rb;

    [Tooltip("Changes the speed of moving left & right")]
    public float speed = 10;

    [Tooltip("Changes the distance the player jumps")]
    public float jumpHeight = 10;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveY = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(MoveX * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpHeight * 100));
        }
    }


    
}
