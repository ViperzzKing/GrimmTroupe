using UnityEngine;
using UnityEngine.SceneManagement;

public class ifTouchDIE : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
