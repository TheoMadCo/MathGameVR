using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager2 : MonoBehaviour
{
    // Canvases for different menu states
    public GameObject welcomeBackground;
    public GameObject gameSelectionBackground;
    public GameObject selectOperationBackground;
    public GameObject difficultySelectionBackground;
    public GameObject confirmPlayBackground;

    private string selectedGame; // Stores the selected game (Fraction or Ordering)
    private string selectedOperation; // Stores the selected fraction operation (Addition, Subtraction, etc.)
    private string selectedDifficulty; // Stores the selected difficulty (Easy, Medium, Hard)

    private Stack<GameObject> navigationStack = new Stack<GameObject>(); // Stack to manage the back button navigation

    void Start()
    {
        // Start by showing the Welcome screen
        ShowWelcomeScreen();
    }

    // Show the Welcome Screen
    public void ShowWelcomeScreen()
    {
        SetActiveScreen(welcomeBackground);
    }

    // Function when the "Play" button is pressed from Welcome Screen
    public void OnPlayPressed()
    {
        SetActiveScreen(gameSelectionBackground);
    }

    // Function for selecting FractionGame or OrderingGame
    public void OnSelectGame(string game)
    {
        selectedGame = game;

        // If the player selects FractionGame, show the SelectOperation screen
        if (selectedGame == "FractionGame")
        {
            SetActiveScreen(selectOperationBackground); // Show operation selection for fraction game
        }
        else if (selectedGame == "OrderingGame")
        {
            // If the player selects OrderingGame, go directly to Difficulty Selection
            SetActiveScreen(difficultySelectionBackground);
        }
    }

    // Function for selecting a fraction operation (only applicable for FractionGame)
    public void OnSelectOperation(string operation)
    {
        selectedOperation = operation;
        SetActiveScreen(difficultySelectionBackground); // Move to difficulty selection after choosing operation
    }

    // Function for selecting a difficulty (common for both games)
    public void OnSelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        SetActiveScreen(confirmPlayBackground); // Move to confirmation screen after selecting difficulty
    }

    // Function to confirm and start the game
    public void OnConfirmPlay()
    {
        if (!string.IsNullOrEmpty(selectedGame) && !string.IsNullOrEmpty(selectedDifficulty))
        {
            // Pass the difficulty and game-specific parameters to the next scene
            PlayerPrefs.SetString("Difficulty", selectedDifficulty);

            if (selectedGame == "FractionGame")
            {
                // If FractionGame, also pass the selected operation
                PlayerPrefs.SetString("Operation", selectedOperation);
                SceneManager.LoadScene("FractionGameScene");
            }
            else if (selectedGame == "OrderingGame")
            {
                SceneManager.LoadScene("OrderingGameScene");
            }
        }
        else
        {
            Debug.LogError("Game or Difficulty not selected.");
        }
    }

    // Function to handle the back button using the navigation stack
    public void OnBackPressed()
    {
        if (navigationStack.Count > 1) // Ensure there is a previous screen to go back to
        {
            navigationStack.Pop(); // Remove the current screen
            GameObject previousScreen = navigationStack.Peek(); // Get the previous screen
            SetActiveScreen(previousScreen, false); // Display the previous screen without pushing it back to the stack
        }
    }

    // Utility to set the active screen and push the current screen onto the navigation stack
    private void SetActiveScreen(GameObject newScreen, bool pushToStack = true)
    {
        // Disable all backgrounds
        ResetAllBackgrounds();

        // Enable the new screen
        newScreen.SetActive(true);

        // Push the current screen to the stack if required
        if (pushToStack)
        {
            navigationStack.Push(newScreen);
        }
    }

    // Quit the game from the Welcome screen
    public void OnExitPressed()
    {
        Application.Quit();
    }

    // Utility to disable all backgrounds
    private void ResetAllBackgrounds()
    {
        welcomeBackground.SetActive(false);
        gameSelectionBackground.SetActive(false);
        selectOperationBackground.SetActive(false);
        difficultySelectionBackground.SetActive(false);
        confirmPlayBackground.SetActive(false);
    }
}