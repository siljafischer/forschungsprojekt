using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // When LoginButton pressed
    public void OnLoginPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
