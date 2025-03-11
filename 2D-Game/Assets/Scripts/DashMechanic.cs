using System.Collections;
using UnityEngine;

public class DashMechanic : MonoBehaviour
{
    public bool collectedCloak = true;


    Rigidbody2D rb;

    [Tooltip("How far the dodge will go")]
    public float dashingPower = 24f;

    public float dashingTime = 0.2f;

    [Tooltip("If the dodge key shall be spammed or not")]
    public float dashingCooldown = 1f;

    [Tooltip("When dodging the amount of frame the player is invincible")]
    public float iFrames;

    private bool canDash = true;
    public bool isDashing;

    [SerializeField] private TrailRenderer tr;

    private float lastDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {

            lastDirection = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
        }

        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

    }


    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(lastDirection * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
