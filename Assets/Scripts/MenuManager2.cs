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

    void Start()
    {
        // Start by showing the Welcome screen
        ShowWelcomeScreen();
    }

    // Show the Welcome Screen
    public void ShowWelcomeScreen()
    {
        ResetAllBackgrounds();
        welcomeBackground.SetActive(true);
    }

    // Function when the "Play" button is pressed from Welcome Screen
    public void OnPlayPressed()
    {
        ResetAllBackgrounds();
        gameSelectionBackground.SetActive(true);
    }

    // Function for selecting FractionGame or OrderingGame
    public void OnSelectGame(string game)
    {
        selectedGame = game;

        // If the player selects FractionGame, show the SelectOperation screen
        if (selectedGame == "FractionGame")
        {
            ResetAllBackgrounds();
            selectOperationBackground.SetActive(true); // Show operation selection for fraction game
        }
        else if (selectedGame == "OrderingGame")
        {
            // If the player selects OrderingGame, go directly to Difficulty Selection
            ResetAllBackgrounds();
            difficultySelectionBackground.SetActive(true);
        }
    }

    // Function for selecting a fraction operation (only applicable for FractionGame)
    public void OnSelectOperation(string operation)
    {
        selectedOperation = operation;
        ResetAllBackgrounds();
        difficultySelectionBackground.SetActive(true); // Move to difficulty selection after choosing operation
    }

    // Function for selecting a difficulty (common for both games)
    public void OnSelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        ResetAllBackgrounds();
        confirmPlayBackground.SetActive(true); // Move to confirmation screen after selecting difficulty
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

    // Function for handling "Back" button in ConfirmPlay screen
    public void OnBackPressed()
    {
        // Go back to Difficulty Selection
        ResetAllBackgrounds();
        difficultySelectionBackground.SetActive(true);
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
