using UnityEngine;
using TMPro;
using System.Diagnostics;

public class GameManager_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public UIManager_CompleteOperation uiManager;
    public ValidationSystem_CompleteOperation validationSystem;
    public SlotManager_CompleteOperation slotManager;

    public int totalTasks = 5;  // Total number of tasks to complete (set in Inspector)
    private int completedTasks = 0;  // Number of tasks completed
    private Stopwatch stopwatch;  // Timer to track time taken
    public string playerName = "Player";  // Set in Inspector for now

    public TextMeshPro displayText;  // Single TextMeshPro for task counter and final score

    void Start()
    {
        // Initialize variables
        completedTasks = 0;
        stopwatch = new Stopwatch();
        stopwatch.Start();

        // Generate and display the first operation
        GenerateNewOperation();

        // Update the UI to show the initial task counter
        UpdateDisplayText();
    }

    private void GenerateNewOperation()
    {
        // Clear all cards from the slots before generating a new operation
        slotManager.ClearAllSlots(); 

        // Generate and display a new operation
        operationGenerator.GenerateNewOperation();
        uiManager.DisplayOperation(operationGenerator.OperationString);
    }

    public void OnCheckResultButtonPressed()
    {
        // Validate the result
        bool isCorrect = validationSystem.ValidateResult();
        uiManager.DisplayResult(isCorrect);  // Show "You Win" or "You Lose"

        if (isCorrect)
        {
            completedTasks++;  // Increment tasks on correct answer
            UpdateDisplayText();

            if (completedTasks >= totalTasks)
            {
                EndGame();  // End the game when all tasks are completed
            }
            else
            {
                GenerateNewOperation();  // Generate the next task
            }
        }
    }

    private void UpdateDisplayText()
    {
        // Update the display text dynamically
        if (completedTasks < totalTasks)
        {
            displayText.text = $"Hai completato {completedTasks} operazioni su {totalTasks}.\n\n Ti rimangono {totalTasks - completedTasks} operazioni da completare!";
        }
    }

    private void EndGame()
    {
        if (displayText == null)
        {
            UnityEngine.Debug.LogError("DisplayText is not assigned in the Inspector.");
            return;
        }

        if (Leaderboard_CompleteOperation.Instance == null)
        {
            UnityEngine.Debug.LogError("Leaderboard_CompleteOperation.Instance is null. Ensure the Leaderboard_CompleteOperation GameObject is in the scene.");
            return;
        }

        stopwatch.Stop();  // Stop the timer
        float completionTime = (float)stopwatch.Elapsed.TotalSeconds;

        // Save the session to the leaderboard
        Leaderboard_CompleteOperation.Instance.AddEntry(playerName, "Complete Operation", completionTime, completedTasks, totalTasks);

        // Export leaderboard to CSV
        Leaderboard_CompleteOperation.Instance.ExportToCSV();

        // Display the final score
        displayText.text = $"Hai completato {completedTasks} operazioni in {completionTime:F2} secondi!"; // Time just for testing purposes
        UnityEngine.Debug.Log("Game ended successfully, and final text was updated.");
    }

}
