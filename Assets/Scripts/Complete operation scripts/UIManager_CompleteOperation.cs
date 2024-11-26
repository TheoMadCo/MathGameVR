using UnityEngine;
using TMPro;

public class UIManager_CompleteOperation : MonoBehaviour
{
    public TextMeshPro operationText;  // Reference to the UI Text that shows the operation
    public TextMeshPro resultText;     // Reference to the UI Text that shows winning or losing messages
    public TextMeshPro scoreText;      // Reference to the UI Text that shows the player's score
    public GameObject shortLine;  // Shorter line for easy/medium levels
    public GameObject longLine;   // Longer line for hard level

    // Pool of messagges in italian both for winning and for loosing that incourages the player to try again
    private string[] winMessages = new string[] { "Bravo! Completa la prossima operazione", "Ottimo lavoro! Completa la prossima operazione", "Fantastico! Completa la prossima operazione", "Continua cosí! Completa la prossima operazione", "Stupendo! Completa la prossima operazione" };
    private string[] loseMessages = new string[] { "Non arrenderti! Ricontrolla le caselle evidenziate in rosso", "Continua a provare! Guarda bene i numeri nelle caselle rosse", "Riprova! Dai una occhiata alle caselle rosse", "Ci sei quasi! Guarda bene le caselle rosse ", "Non mollare! Ritenta e guarda bene le caselle rosse" };

    // Displays the operation on the UI
    public void DisplayOperation(string operation)
    {
        operationText.text = operation;
    }

    // Configures the visibility of lines based on difficulty
    public void UpdateLinesForDifficulty(Difficulty difficulty)
    {
        // Toggle line visibility
        if (difficulty == Difficulty.Hard)
        {
            shortLine.SetActive(false);
            longLine.SetActive(true);
        }
        else
        {
            shortLine.SetActive(true);
            longLine.SetActive(false);
        }

    }

    // Updates the display text for score or operations completed
    public void UpdateDisplayText(int completedTasks, int totalTasks)
    {
        scoreText.text = $"Hai completato {completedTasks} operazioni su {totalTasks}.\n\n Ti rimangono {totalTasks - completedTasks} operazioni da completare!";
    }
    
    // Displays the final score with the number of completed tasks and completion time
    public void DisplayFinalScore(int completedTasks, float completionTime)
    {
        scoreText.text = $"Hai completato {completedTasks} operazioni in {completionTime:F2} secondi!";
    }

    // Displays the endgame message when the game is completed
    public void DisplayEndGameMessage()
    {
        resultText.text = "Complimenti, hai completato il gioco!";
    }

    // Displays whether the player wins or loses a round
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
