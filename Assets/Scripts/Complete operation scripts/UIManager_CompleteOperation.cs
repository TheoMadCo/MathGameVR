using UnityEngine;
using TMPro;

public class UIManager_CompleteOperation : MonoBehaviour
{
    public TextMeshPro operationText;  // Reference to the UI Text that shows the operation
    public TextMeshPro resultText;     // Reference to the UI Text that shows "You Win" or "You Lose"
    // Pool of messagges in italian both for winning and for loosing that incourages the player to try again
    private string[] winMessages = new string[] { "Bravo!", "Ottimo lavoro!", "Fantastico!", "Eccellente!", "Stupendo!" };
    private string[] loseMessages = new string[] { "Non arrenderti! Ricontrolla le caselle evidenziate in rosso", "Continua a provare! Guarda bene i numeri nelle caselle rosse", "Riprova! Dai una occhiata alle caselle rosse", "Ci sei quasi! Guarda bene le caselle rosse ", "Non mollare! Ritenta e guarda bene le caselle rosse" };

    // Displays the operation on the UI
    public void DisplayOperation(string operation)
    {
        operationText.text = operation;
    }

    // Displays whether the player wins or loses
    public void DisplayResult(bool isCorrect)
    {
        if (isCorrect)
        {
            resultText.text = winMessages[Random.Range(0, winMessages.Length)];
        }
        else
        {
            resultText.text = loseMessages[Random.Range(0, loseMessages.Length)];
        }
    }
}
