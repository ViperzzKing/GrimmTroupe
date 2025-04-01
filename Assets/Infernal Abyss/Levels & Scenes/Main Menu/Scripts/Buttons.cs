using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void Play()
    {
        // SceneManager.LoadScene("Story");
        Debug.Log("Story Loaded");
    }
    public void Options()
    {
        Debug.Log("Options Opened");
    }
    public void Quit()
    {
        Debug.Log("Unity is self destructing in 5, 4...");
    }
}
