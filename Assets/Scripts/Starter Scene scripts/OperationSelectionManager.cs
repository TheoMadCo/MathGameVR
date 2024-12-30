using UnityEngine;
using UnityEngine.SceneManagement;

public class OperationSelectionManager : MonoBehaviour
{
    public static string selectedOperation;
    public static string difficultyLevel;

    public void SelectAddition()
    {
        selectedOperation = "Addition";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    public void SelectSubtraction()
    {
        selectedOperation = "Subtraction";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    public void SelectMultiplication()
    {
        selectedOperation = "Multiplication";
        SceneManager.LoadScene("DifficultySelectionScene");
    }

    public void SelectDivision()
    {
        selectedOperation = "Division";
        SceneManager.LoadScene("DifficultySelectionScene");
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartupScene");
    }
}


