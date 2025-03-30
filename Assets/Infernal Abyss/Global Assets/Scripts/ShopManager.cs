using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int currencyAmount;

    PlayerMovementStateMachine movement;

    private void Start()
    {
        movement = FindFirstObjectByType<PlayerMovementStateMachine>();
    }

    public void BuyDash()
    {
        Debug.Log("Button Clicked");
        if (currencyAmount >= 10)
        {
            currencyAmount -= 10;
            movement.dashCollected = true;
        }
        else
        {
            Debug.Log("Not Enough MONEH");
        }
    }

    void GiveCoins()
    {
        currencyAmount += 5;
    }
}
