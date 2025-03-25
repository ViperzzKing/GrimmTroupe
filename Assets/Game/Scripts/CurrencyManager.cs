using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
<<<<<<< HEAD
    public int currentCoins;
    private int minCoins;

    PlayerMovementStateMachine movement;

    private void Start()
    {
        movement = FindFirstObjectByType<PlayerMovementStateMachine>();
    }

    private void Update()
    {
        if(currentCoins <= minCoins)
        {
            currentCoins = minCoins;
=======
    public int currentMoney;
    public int minMoney = 0;

    private void Update()
    {
        if(currentMoney <= 0)
        {
            currentMoney = 0;
>>>>>>> 2c3f9e9051ad2fbfdd072b17a26ca1e0ce4e94fe
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< HEAD
        currentCoins += 1;
        Destroy(gameObject);
    }

    public void BuyDash()
    {
        currentCoins -= 10;
        movement.dashCollected = true;
    }

    public void GiveCoins()
    {
        currentCoins += 5;
=======
        currentMoney += 1;
>>>>>>> 2c3f9e9051ad2fbfdd072b17a26ca1e0ce4e94fe
    }
}
