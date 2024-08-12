using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectionManager : MonoBehaviour
{
    public void SelectEasy()
    {
        OperationSelectionManager.difficultyLevel = "Easy";
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMedium()
    {
        OperationSelectionManager.difficultyLevel = "Medium";
        SceneManager.LoadScene("GameScene");
    }

    public void SelectHard()
    {
        OperationSelectionManager.difficultyLevel = "Hard";
        SceneManager.LoadScene("GameScene");
    }
    public void BackToSelection()
    {
        SceneManager.LoadScene("SelectionScene");
    }
}
