using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class OrderGameManagerHardcoded : MonoBehaviour
{
    public string difficulty = "easy"; //difficulty selection only for test purposes

    public GameObject blockPrefab;  // Prefab for numbered block
    public GameObject platePrefab;  // Prefab for the plate with XR Socket Interactor
    public Transform[] blockSpawnPoints;  // Array of predefined spawn points for blocks
    public Transform[] plateSpawnPoints;  // Array of predefined spawn points for plates

    public int easyBlockCount = 3;  // Number of blocks/plates for easy difficulty
    public int mediumBlockCount = 5;  // Number of blocks/plates for medium difficulty
    public int hardBlockCount = 8;  // Number of blocks/plates for hard difficulty

    // UI Elements
    public TextMeshPro resultText;  // Reference to the result text on the canvas
    public Button checkSolutionButton;  // Reference to the "Check Solution" button
    public Button panicButton;  // Reference to the "Panic" button
    public Color enabledColor = Color.green;  // Color when button is enabled
    public Color disabledColor = Color.grey;  // Color when button is disabled

    private List<GameObject> spawnedPlates = new List<GameObject>();  // Store spawned plates
    private int numberOfPlates;  // Number of plates to generate, based on difficulty
    private bool isAscendingOrder;  // True if ascending order, false if descending

    // Audio clips for correct and incorrect order
    public AudioClip correctOrderClip;
    public AudioClip incorrectOrderClip;

    void Start()
    {
        SetDifficulty(difficulty);  // Set default difficulty (you can change this dynamically)
        GeneratePlatesAndBlocks();  // Call this to spawn the plates and blocks

        // Set the button to be unclickable at the start and show as grey
        SetButtonInteractable(false);
    }

    // Set the number of plates based on difficulty
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
                numberOfPlates = easyBlockCount;  // Default to easy
                break;
        }

        // Update the result text to show the current difficulty and sorting order
        isAscendingOrder = Random.value > 0.5f;
        string sortingOrder = isAscendingOrder ? "Ascending" : "Descending";
        resultText.text = $"Difficulty: {difficulty} - Order in {sortingOrder}";
    }

    // Called when a block is placed in a socket
    public void OnBlockPlaced(SelectEnterEventArgs args)
    {
        CheckIfAllBlocksPlaced();
    }

    // Called when a block is removed from a socket
    public void OnBlockRemoved(SelectExitEventArgs args)
    {
        CheckIfAllBlocksPlaced();
    }


    // Method to generate plates and blocks based on difficulty
    public void GeneratePlatesAndBlocks()
    {
        // Clear any previously spawned plates
        foreach (var plate in spawnedPlates) Destroy(plate);
        spawnedPlates.Clear();

        // Center-align plates by finding the midpoint
        int totalSpawnPoints = plateSpawnPoints.Length;
        int startingIndex = (totalSpawnPoints - numberOfPlates) / 2;  // Calculate starting index for centering

        // Spawn plates at the centered spawn points
        for (int i = 0; i < numberOfPlates; i++)
        {
            // Find the plate spawn point, ensuring we start at the center
            Transform spawnPoint = plateSpawnPoints[startingIndex + i];

            // Instantiate the plate at the spawn point
            GameObject plate = Instantiate(platePrefab, spawnPoint.position, Quaternion.identity);
            spawnedPlates.Add(plate);  // Store the spawned plate in the list

            // Get the XRSocketInteractor of the plate and subscribe to the socket events
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket != null)
            {
                // Listen for when a block is placed or removed from the socket
                socket.selectEntered.AddListener(OnBlockPlaced);
                socket.selectExited.AddListener(OnBlockRemoved);
            }
        }

        // Now generate the blocks at the corresponding block spawn points
        GenerateBlocks();

        // Disable the "Check Solution" button until all blocks are placed
        SetButtonInteractable(false);
    }


    // Method to generate blocks (same as before)
    public void GenerateBlocks()
    {
        // Calculate the starting index for centering the blocks
        int totalSpawnPoints = blockSpawnPoints.Length;
        int startingIndex = (totalSpawnPoints - numberOfPlates) / 2;

        for (int i = 0; i < numberOfPlates; i++)
        {
            if (i < blockSpawnPoints.Length) // Check if we have enough spawn points
            {
                // Find the block spawn point, ensuring we start at the center
                Transform spawnPoint = blockSpawnPoints[startingIndex + i];

                GameObject block = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
                block.GetComponent<BlockController>().SetBlockNumber(Random.Range(1, 100));
            }
        }
    }

    // Check if all blocks are placed and enable the "Check Solution" button
    public void CheckIfAllBlocksPlaced()
    {
        bool allBlocksPlaced = true;

        // Loop through all plates and check if they have blocks in the sockets
        foreach (GameObject plate in spawnedPlates)
        {
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket == null || !socket.hasSelection)
            {
                allBlocksPlaced = false;
                Debug.Log("A block is missing in one of the sockets.");
                break;
            }
        }

        Debug.Log("All blocks placed: " + allBlocksPlaced);

        // Enable the "Check Solution" button if all blocks are placed, disable it otherwise
        SetButtonInteractable(allBlocksPlaced);
    }


    // Called when the check button is pressed
    public void OnCheckSolutionPressed()
    {
        Debug.Log("Check Solution button pressed!");
        CheckSolution();
    }

    // Method to check if the blocks are placed in the correct order
    public void CheckSolution()
    {
        List<int> blockNumbers = new List<int>();

        // Check all the plates to see if blocks have been placed
        foreach (GameObject plate in spawnedPlates)
        {
            XRSocketInteractor socket = plate.GetComponentInChildren<XRSocketInteractor>();
            if (socket != null && socket.hasSelection)
            {
                // Get the block that is placed in the socket
                BlockController block = socket.GetOldestInteractableSelected().transform.GetComponent<BlockController>();
                if (block != null)
                {
                    blockNumbers.Add(block.GetBlockNumber());
                }
            }
        }

        // Check if the block numbers are in the correct order (ascending/descending)
        bool correctOrder = isAscendingOrder ? IsSortedAscending(blockNumbers) : IsSortedDescending(blockNumbers);

        // Display result based on whether the order is correct
        //if the result is correct, put "Well Done!" in the result text, otherwise put "Try Again!". Basing on the result, a different sound will be played.
        if (correctOrder)
        {
            resultText.text = "Well Done!";
        }
        else
        {
            resultText.text = "Try Again!";
            
        }

        // Play the corresponding audio clip based on the result
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(correctOrder ? correctOrderClip : incorrectOrderClip, 1);

        //Remove the "panic" button
        panicButton.gameObject.SetActive(false);

        // Update the button text to "Retry"
        checkSolutionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";

        // Change the OnClick function to restart the game using SceneManager
        checkSolutionButton.onClick.RemoveAllListeners();  // Clear any previous listeners
        checkSolutionButton.onClick.AddListener(RestartGame);  // Assign the restart function to the button
    }


    // Helper method to check if the block numbers are in ascending order
    private bool IsSortedAscending(List<int> numbers)
    {
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] > numbers[i + 1])
                return false;
        }
        return true;
    }

    // Helper method to check if the block numbers are in descending order
    private bool IsSortedDescending(List<int> numbers)
    {
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] < numbers[i + 1])
                return false;
        }
        return true;
    }

    // Method to change the color and interactability of the "Check Solution" button
    private void SetButtonInteractable(bool interactable)
    {
        // Enable/Disable the button's interactivity
        checkSolutionButton.interactable = interactable;

        // Change the color of the button
        ColorBlock colors = checkSolutionButton.colors;
        colors.normalColor = interactable ? enabledColor : disabledColor;
        checkSolutionButton.colors = colors;
    }

    // Function to restart the game and regenerate blocks and plates
    public void RestartGame()
    {
        // Reload the current scene to reset everything
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
