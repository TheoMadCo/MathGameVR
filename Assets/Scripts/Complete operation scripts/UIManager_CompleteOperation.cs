using UnityEngine;
using TMPro;

public class UIManager_CompleteOperation : MonoBehaviour
{
    public TextMeshPro operationText;  // Reference to the UI Text that shows the operation
    public TextMeshPro resultText;     // Reference to the UI Text that shows "You Win" or "You Lose"

    // Displays the operation on the UI
    public void DisplayOperation(string operation)
    {
        operationText.text = operation;
    }

    // Displays whether the player wins or loses
    public void DisplayResult(bool isCorrect)
    {
        resultText.text = isCorrect ? "You Win!" : "You Lose!";
    }
}
