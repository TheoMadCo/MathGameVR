using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryQuitManager : MonoBehaviour
{
    // Function to restart the games
    public void RestartGame()
    {
        // Reload the current scene to reset everything
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Function to go back to StarterSceneClassroom
    public void BackToStarterScene()
    {
        SceneManager.LoadScene("StarterSceneClassroom");
    }
}
