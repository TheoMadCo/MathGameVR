using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
