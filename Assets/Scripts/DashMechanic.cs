using UnityEngine;

public class DashMechanic : MonoBehaviour
{
    [Tooltip("How far the dodge will go")]
    public float dodgePower = 24f;

    public float dashingTime = 0.2f;

    [Tooltip("If the dodge key shall be spammed or not")]
    public float dodgeCooldown = 1f;

    [Tooltip("When dodging the amount of frame the player is invincible")]
    public float iFrames;

    private bool canDash = true;



    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
