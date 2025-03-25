using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region CoinVaribles

    public int coinCount;
    public int coinCurrent;

    #endregion


    void Start()
    {
        coinCurrent = 0;
        coinCount = coinCurrent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount += 1;
        }
    }
}
