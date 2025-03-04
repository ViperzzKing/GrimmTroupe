using UnityEngine;

//tells the game to give object these components
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class playerMovement : MonoBehaviour
{

    private float MoveX;
    private float MoveY;

    public Rigidbody2D rb;

    [Tooltip("Changes the speed of moving left & right")]
    public float speed = 10;

    [Tooltip("Changes the distance the player jumps")]
    public float jumpHeight = 10;

    [Tooltip("How far the dodge will go")]
    public float dodgeDistance;

    [Tooltip("If the dodge key shall be spammed or not")]
    public float dodgeCooldown;

    [Tooltip("When dodging the amount of frame the player is invincible")]
    public float iFrames;


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
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpHeight * Time.deltaTime));
        }
    }


    
}
