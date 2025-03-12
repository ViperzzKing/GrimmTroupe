using System.Collections;
using UnityEngine;

public class DashMechanic : MonoBehaviour
{
    public bool dashCollected = true;

    public float dashingPower = 100f;
    public float dashingTime = 0.5f;
    public float dashingCooldown = .5f;
    public float dashGravity;

    public float noDamageTime;
    public bool noDamage = false;


    private bool canDash = true;
    public bool isDashing;

    public Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCollected)
        {
            StartCoroutine(Dash());
            StartCoroutine(NoDamage());
        }
    }


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

   public IEnumerator NoDamage()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        noDamage = true;
        yield return new WaitForSeconds(noDamageTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        noDamage = false;
    }
}
