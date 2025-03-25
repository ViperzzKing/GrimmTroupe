using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int currentCoins;
    private int minCoins;

    PlayerMovementStateMachine movement;

    private void Start()
    {
        movement = FindFirstObjectByType<PlayerMovementStateMachine>();
    }

    private void Update()
    {
        if (currentCoins <= minCoins)
        {
            currentCoins = minCoins;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentCoins += 1;
        Destroy(gameObject);
    }

    void BuyDash()
    {
        currentCoins -= 10;
        movement.dashCollected = true;
    }

    void GiveCoins()
    {
        currentCoins += 5;
    }
}
