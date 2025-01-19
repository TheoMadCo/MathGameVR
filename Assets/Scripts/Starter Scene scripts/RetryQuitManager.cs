using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryQuitManager : MonoBehaviour
{
    // Function to restart the games, preserving PlayerPrefs values
    public void RestartGame()
    {
        // Ensure PlayerPrefs values are preserved and reload the scene
        Debug.Log("Restarting game. PlayerPrefs retained.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Function to go back to StarterSceneClassroom
    public void BackToStarterScene()
    {
        // Clear PlayerPrefs for the operation and difficulty
        PlayerPrefs.DeleteKey("Operation");
        PlayerPrefs.DeleteKey("Difficulty");

        // Set a flag to indicate we're returning from a game
        PlayerPrefs.SetInt("ReturnToGameSelection", 1);

        Debug.Log("Returning to starter scene. PlayerPrefs cleared for Operation and Difficulty.");
        SceneManager.LoadScene("StarterSceneClassroom");
    }


}
