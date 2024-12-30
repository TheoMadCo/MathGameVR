using UnityEngine;
using System.Diagnostics;

public enum Difficulty { Easy, Medium, Hard }

public class GameManager_CompleteOperation : MonoBehaviour
{
    [Header("Scripts")]
    public OperationGenerator operationGenerator;
    public UIManager_CompleteOperation uiManager;
    public ValidationSystem_CompleteOperation validationSystem;
    public SlotManager_CompleteOperation slotManager;

    [Header("Game Parameters")]
    public int totalTasks = 5;  // Total number of tasks to complete (set in Inspector)
    public Difficulty selectedDifficulty = Difficulty.Easy;  // Set difficulty in Inspector
    private int completedTasks = 0;  // Number of tasks completed
    private Stopwatch stopwatch;  // Timer to track time taken
    public string playerName = "Player";  // Set in Inspector for now

    [Header("Optional overrides for testing")]
    public OperationType? operationTypeOverride;  // Allows testing specific operations
    public Difficulty? difficultyOverride;        // Allows testing specific difficulties

    private bool operationSolvedCorrectly = false;  // Flag to track if current operation was solved correctly

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

        // Clear any previous highlights
        slotManager.ClearAllHighlights();
    }

    public void OnCheckResultButtonPressed()
    {
        // Validate the result
        bool isCorrect = validationSystem.ValidateResult();

        if (isCorrect)
        {
            // Set flag for correct operation
            operationSolvedCorrectly = true;

            // Highlight correct slots in green
            foreach (int correctSlotIndex in validationSystem.correctSlotIndices)
            {
                slotManager.HighlightCorrectSlot(correctSlotIndex);
            }

            // Display result with continue button
            uiManager.DisplayResult(isCorrect);
        }
        else
        {
            // For incorrect result, just show the result
            uiManager.DisplayResult(isCorrect);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the continue button event from UIManager
        uiManager.OnContinueButtonPressed += HandleContinueButtonPressed;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        uiManager.OnContinueButtonPressed -= HandleContinueButtonPressed;
    }

    private void HandleContinueButtonPressed()
    {
        if (operationSolvedCorrectly)
        {
            // Reset the flag
            operationSolvedCorrectly = false;

            // Increment completed tasks
            completedTasks++;
            uiManager.UpdateDisplayText(completedTasks, totalTasks);

            // Prepare UI for next operation
            uiManager.PrepareForNextOperation();

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

        // Clear all cards from the slots 
        slotManager.ClearAllSlots();

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
        uiManager.DisplayFinalScore(completedTasks);
        uiManager.ShowFinalScoreCanvas();

        UnityEngine.Debug.Log("Game ended successfully, and final text was updated.");
    }
}