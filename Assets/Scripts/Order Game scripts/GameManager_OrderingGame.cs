using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager_OrderingGame : MonoBehaviour
{
    [Header("Game Difficulty Settings")]
    private string selectedDifficulty; // Difficulty fetched from PlayerPrefs
    public int easyBlockCount = 3;
    public int mediumBlockCount = 5;
    public int hardBlockCount = 8;

    [Header("Prefabs and Spawn Points")]
    public GameObject blockPrefab;
    public GameObject platePrefab;
    public Transform[] blockSpawnPoints;
    public Transform[] plateSpawnPoints;

    [Header("Turn Management")]
    public int totalTurns = 3;
    public int currentTurn = 1;

    [Header("Debug")]
    public bool enableDebugLogging = true;

    // Reference to UI Manager
    private UIManager_OrderingGame uiManager;

    private List<GameObject> spawnedPlates = new List<GameObject>();
    private int numberOfPlates;
    private bool isAscendingOrder;

    void Start()
    {
        // Find the UI Manager
        uiManager = FindObjectOfType<UIManager_OrderingGame>();

        // Initialize the game
        selectedDifficulty = PlayerPrefs.GetString("Difficulty", "Easy");

        // Initialize UI
        uiManager.InitializeGameUI(totalTurns);

        SetDifficulty(selectedDifficulty);
        GeneratePlatesAndBlocks();
    }

    private void DebugLog(string message)
    {
        if (enableDebugLogging)
        {
            Debug.Log($"[OrderGameManager] {message}");
        }
    }

    public void SetDifficulty(string difficulty)
    {
        switch (difficulty.ToLower())
        {
            case "easy":
                numberOfPlates = easyBlockCount;
                break;
            case "medium":
                numberOfPlates = mediumBlockCount;
                break;
            case "hard":
                numberOfPlates = hardBlockCount;
                break;
            default:
                numberOfPlates = easyBlockCount;
                break;
        }

        // Randomly decide sorting order
        isAscendingOrder = Random.value > 0.5f;

        // Update UI feedback text
        uiManager.UpdateFeedbackText(difficulty, currentTurn, totalTurns, isAscendingOrder);
    }

    public void GeneratePlatesAndBlocks()
    {
        // Clear previous plates
        foreach (var plate in spawnedPlates)
        {
            // Unsubscribe from events before destroying
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket != null)
            {
                socket.selectEntered.RemoveListener(OnBlockPlaced);
                socket.selectExited.RemoveListener(OnBlockRemoved);
            }
            Destroy(plate);
        }
        spawnedPlates.Clear();

        // Destroy existing blocks
        GameObject[] existingBlocks = GameObject.FindGameObjectsWithTag("Block");
        uiManager.ResetBlockMaterials(existingBlocks);
        foreach (GameObject block in existingBlocks)
        {
            Destroy(block);
        }

        int totalSpawnPoints = plateSpawnPoints.Length;
        int startingIndex = (totalSpawnPoints - numberOfPlates) / 2;

        // Spawn plates
        for (int i = 0; i < numberOfPlates; i++)
        {
            Transform spawnPoint = plateSpawnPoints[startingIndex + i];
            GameObject plate = Instantiate(platePrefab, spawnPoint.position, Quaternion.identity);
            spawnedPlates.Add(plate);

            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket != null)
            {
                socket.selectEntered.AddListener(OnBlockPlaced);
                socket.selectExited.AddListener(OnBlockRemoved);
            }
        }

        // Generate blocks
        GenerateBlocks();

        // Disable the "Check Solution" button
        uiManager.SetButtonInteractable(false);
    }

    public void GenerateBlocks()
    {
        int totalSpawnPoints = blockSpawnPoints.Length;
        int startingIndex = (totalSpawnPoints - numberOfPlates) / 2;

        for (int i = 0; i < numberOfPlates; i++)
        {
            if (i < blockSpawnPoints.Length)
            {
                Transform spawnPoint = blockSpawnPoints[startingIndex + i];
                GameObject block = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);

                // Tag the block for easy finding and destruction
                block.tag = "Block";

                block.GetComponent<BlockController>().SetBlockNumber(Random.Range(1, 100));
            }
        }
    }

    public void OnBlockPlaced(SelectEnterEventArgs args)
    {
        CheckIfAllBlocksPlaced();
    }

    public void OnBlockRemoved(SelectExitEventArgs args)
    {
        CheckIfAllBlocksPlaced();
    }

    public void CheckIfAllBlocksPlaced()
    {
        bool allBlocksPlaced = true;

        foreach (GameObject plate in spawnedPlates)
        {
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket == null || !socket.hasSelection)
            {
                allBlocksPlaced = false;
                break;
            }
        }

        uiManager.SetButtonInteractable(allBlocksPlaced);
    }

    public void OnCheckSolutionPressed()
    {
        CheckSolution();
    }

    public void CheckSolution()
    {
        List<int> blockNumbers = new List<int>();
        List<GameObject> blockObjects = new List<GameObject>();

        // Collect block numbers and objects in order
        foreach (GameObject plate in spawnedPlates)
        {
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket != null && socket.hasSelection)
            {
                BlockController block = socket.GetOldestInteractableSelected().transform.GetComponent<BlockController>();
                if (block != null)
                {
                    blockNumbers.Add(block.GetBlockNumber());
                    blockObjects.Add(block.gameObject);
                }
            }
        }

        // Debug logging for input numbers
        string inputNumbersDebug = $"Input Numbers ({(isAscendingOrder ? "Ascending" : "Descending")}): {string.Join(", ", blockNumbers)}";
        DebugLog(inputNumbersDebug);

        // Comprehensive solution check
        bool[] blockPositions = GetBlockPositions(blockNumbers);
        bool isCompleteSolutionCorrect = IsCompleteSolutionCorrect(blockNumbers, blockPositions);

        // Color blocks based on their correctness
        uiManager.ColorBlocks(blockObjects.ToArray(), blockPositions);

        if (isCompleteSolutionCorrect)
        {
            uiManager.HandleCorrectSolution(currentTurn, totalTurns);
        }
        else
        {
            uiManager.HandleIncorrectSolution(isAscendingOrder);
        }
    }

    private List<int> GenerateExpectedOrder(List<int> inputNumbers)
    {
        // Create a copy of the input list to avoid modifying the original
        List<int> sortedNumbers = new List<int>(inputNumbers);

        // Sort based on the current sorting order
        if (isAscendingOrder)
        {
            sortedNumbers.Sort(); // Ascending
        }
        else
        {
            sortedNumbers.Sort((a, b) => b.CompareTo(a)); // Descending
        }

        return sortedNumbers;
    }

    private bool[] GetBlockPositions(List<int> inputNumbers)
    {
        // Generate the expected order
        List<int> expectedOrder = GenerateExpectedOrder(inputNumbers);

        // Create an array to track correct positions
        bool[] correctPositions = new bool[inputNumbers.Count];

        // Compare each input number with its expected position
        for (int i = 0; i < inputNumbers.Count; i++)
        {
            // Check if the current number is in the correct position
            correctPositions[i] = inputNumbers[i] == expectedOrder[i];
        }

        return correctPositions;
    }

    private bool IsCompleteSolutionCorrect(List<int> numbers, bool[] blockPositions)
    {
        // First, check if all block positions are correct
        foreach (bool isCorrect in blockPositions)
        {
            if (!isCorrect)
            {
                return false;
            }
        }

        // Then, perform a global order check
        List<int> expectedOrder = GenerateExpectedOrder(numbers);

        // Compare the input order with the expected order
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] != expectedOrder[i])
            {
                DebugLog($"Position {i} is incorrect. Expected {expectedOrder[i]}, got {numbers[i]}");
                return false;
            }
        }

        return true;
    }

    public void ContinueToNextTurn()
    {
        currentTurn++;

        if (currentTurn <= totalTurns)
        {
            // Reset UI for next turn
            uiManager.ResetTurnUI();

            // Regenerate plates and blocks
            SetDifficulty(selectedDifficulty);
            GeneratePlatesAndBlocks();
        }
        else
        {
            // Game completed, show final result canvas
            uiManager.ShowFinalResultCanvas();
        }
    }
}