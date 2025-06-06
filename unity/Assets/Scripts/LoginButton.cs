using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    // When LoginButton pressed
    public void OnLoginPressed()
    {
        SceneManager.LoadScene("first_steps");
    }
}
