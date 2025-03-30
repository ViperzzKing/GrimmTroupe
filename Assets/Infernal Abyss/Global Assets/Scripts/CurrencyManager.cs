using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int minCoins;

    ShopManager coins;

    private void Start()
    {
        coins = FindFirstObjectByType<ShopManager>();
    }

    private void Update()
    {
        if (coins.currencyAmount <= minCoins)
        {
            coins.currencyAmount = minCoins;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coins.currencyAmount += 1;
        // Destroy(gameObject);
    }
}
