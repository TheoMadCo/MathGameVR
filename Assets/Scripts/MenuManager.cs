using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject startCanvas;
    public GameObject gameSelectionCanvas;
    public GameObject difficultySelectionCanvas;
    public GameObject startGameCanvas;

    void Start()
    {
        // Set the initial state where only StartCanvas is active
        TransitionToCanvas(startCanvas);
    }

    public void OnStartGamePressed()
    {
        TransitionToCanvas(gameSelectionCanvas);
    }

    public void OnStartPlayNowPressed()
    {
        // Load the actual game scene
        SceneManager.LoadScene("GameScene");
    }

    // Generic method to transition between canvases
    private void TransitionToCanvas(GameObject targetCanvas)
    {
        startCanvas.SetActive(false);
        gameSelectionCanvas.SetActive(false);
        difficultySelectionCanvas.SetActive(false);
        startGameCanvas.SetActive(false);

        targetCanvas.SetActive(true);
    }

    public void OnBackToStartupPressed()
    {
        TransitionToCanvas(startCanvas);
    }

    public void OnBackToGameSelectionPressed()
    {
        TransitionToCanvas(gameSelectionCanvas);
    }

    public void OnBackToDifficultySelectionPressed()
    {
        TransitionToCanvas(difficultySelectionCanvas);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }

}
