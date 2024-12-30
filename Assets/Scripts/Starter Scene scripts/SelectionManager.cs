using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static string selectedOperation;
    public static string selectedDifficulty;

    public GameObject gameSelectionCanvas;
    public GameObject difficultySelectionCanvas;
    public GameObject startGameCanvas;

    // Handles the selection of the operation (Addition, Subtraction, etc.)
    public void SelectOperation(string operation)
    {
        selectedOperation = operation;
        TransitionToCanvas(difficultySelectionCanvas);
    }

    // Handles the selection of the difficulty (Easy, Medium, Hard)
    public void SelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        TransitionToCanvas(startGameCanvas);
    }

    // Generic method to transition between canvases
    private void TransitionToCanvas(GameObject targetCanvas)
    {
        gameSelectionCanvas.SetActive(false);
        difficultySelectionCanvas.SetActive(false);
        startGameCanvas.SetActive(false);

        targetCanvas.SetActive(true);
    }
}
