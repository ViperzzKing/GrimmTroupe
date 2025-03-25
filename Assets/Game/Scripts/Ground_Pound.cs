using UnityEngine;

public class Grond_Pound : MonoBehaviour
{
    public float poundForce = -24f;
    private Rigidbody rb;
    public bool hasGroundPound = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && hasGroundPound) { Jump(); }
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.angularVelocity.x, poundForce, rb.angularVelocity.y);
    }
}
