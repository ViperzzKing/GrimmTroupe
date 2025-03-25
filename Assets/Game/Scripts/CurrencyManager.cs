using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int currentMoney;
    public int minMoney = 0;

    private void Update()
    {
        if(currentMoney <= 0)
        {
            currentMoney = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentMoney += 1;
    }
}
