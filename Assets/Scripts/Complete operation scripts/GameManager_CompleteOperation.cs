using UnityEngine;
using TMPro;
using System.Diagnostics;

public enum Difficulty { Easy, Medium, Hard }

public class GameManager_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public UIManager_CompleteOperation uiManager;
    public ValidationSystem_CompleteOperation validationSystem;
    public SlotManager_CompleteOperation slotManager;

    public int totalTasks = 5;  // Total number of tasks to complete (set in Inspector)
    public Difficulty selectedDifficulty = Difficulty.Easy;  // Set difficulty in Inspector
    private int completedTasks = 0;  // Number of tasks completed
    private Stopwatch stopwatch;  // Timer to track time taken
    public string playerName = "Player";  // Set in Inspector for now

    // Optional overrides for testing
    public OperationType? operationTypeOverride;  // Allows testing specific operations
    public Difficulty? difficultyOverride;        // Allows testing specific difficulties


    void Start()
    {
        // Fetch and log operation type
        OperationType operationType = operationTypeOverride.HasValue
            ? operationTypeOverride.Value
            : (OperationType)System.Enum.Parse(typeof(OperationType), PlayerPrefs.GetString("Operation", "Addition"));

        UnityEngine.Debug.Log($"Starting GameManager. OperationType: {operationType}");

        // Fetch and log difficulty
        Difficulty difficulty = difficultyOverride.HasValue
            ? difficultyOverride.Value
            : (Difficulty)System.Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString("Difficulty", "Easy"));

        UnityEngine.Debug.Log($"Starting GameManager. Difficulty: {difficulty}");

        // Pass the selected difficulty and operation to other components
        operationGenerator.SetOperationType(operationType);
        operationGenerator.SetDifficulty(difficulty);
        slotManager.ConfigureSlotsForDifficulty(difficulty);

        // Update UI based on difficulty
        uiManager.UpdateLinesForDifficulty(difficulty);

        // Initialize variables
        completedTasks = 0;
        stopwatch = new Stopwatch();
        stopwatch.Start();

        // Generate and display the first operation
        GenerateNewOperation();

        UnityEngine.Debug.Log("First operation generated successfully.");

        // Update the UI to show the initial task counter
        uiManager.UpdateDisplayText(completedTasks, totalTasks);
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
        uiManager.DisplayResult(isCorrect);  // Show winning or losing message

        if (isCorrect)
        {
            completedTasks++;  // Increment tasks on correct answer
            uiManager.UpdateDisplayText(completedTasks, totalTasks);

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


    private void EndGame()
    {
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
        uiManager.DisplayEndGameMessage();
        // Use UIManager to display the final score
        uiManager.DisplayFinalScore(completedTasks, completionTime);
        UnityEngine.Debug.Log("Game ended successfully, and final text was updated.");
    }

}