using UnityEngine;
using UnityEngine.UI; // For the continue button
using TMPro;
using System;

public class UIManager_CompleteOperation : MonoBehaviour
{
    [Header("Text elements")]
    public TextMeshPro operationText;  // Reference to the UI Text that shows the operation
    public TextMeshPro resultText;     // Reference to the UI Text that shows winning or losing messages
    public TextMeshPro scoreText;      // Reference to the UI Text that shows the player's score
    public TextMeshPro complimentText; // New text for compliment message

    [Header("Lines")]
    public GameObject shortLine;  // Shorter line for easy/medium levels
    public GameObject longLine;   // Longer line for hard level

    [Header("Canvases elements")]
    public GameObject gameCanvas;      // Reference to the main game canvas
    public GameObject finalScoreCanvas; // Reference to the final score canvas
    public GameObject gameButtonCanvas; // Reference to the game button canvas
    public GameObject finalScoreButtonCanvas; // Reference to the endgame button canvas
    public TextMeshPro finalResultText; // Reference to the result text on final score canvas
    public Button continueButton;       // Reference to the continue button

    [Header("Audio Elements")]
    public AudioSource audioSource;     // Audio source component
    public AudioClip winSound;          // Sound played when winning
    public AudioClip loseSound;         // Sound played when losing
    public AudioClip newRoundSound;     // Sound played when winning
    public AudioClip endGameSound;      // Sound played when losing


    // Pool of messagges in italian both for winning and for losing that incourages the player to try again
    private string[] winMessages = new string[] {
        "Bravo! Completa la prossima operazione",
        "Ottimo lavoro! Completa la prossima operazione",
        "Fantastico! Completa la prossima operazione",
        "Continua cosí! Completa la prossima operazione",
        "Stupendo! Completa la prossima operazione"
    };

    private string[] loseMessages = new string[] {
        "Non arrenderti! Ricontrolla le caselle evidenziate in rosso",
        "Continua a provare! Guarda bene i numeri nelle caselle rosse",
        "Riprova! Dai una occhiata alle caselle rosse",
        "Ci sei quasi! Guarda bene le caselle rosse ",
        "Non mollare! Ritenta e guarda bene le caselle rosse"
    };

    private string[] complimentMessages = new string[] {
        "Fantastico! Operazione completata correttamente!",
        "Eccellente! Hai risolto l'operazione alla perfezione!",
        "Complimenti! Sei un vero matematico!",
        "Bravissimo! Hai completato l'operazione con successo!",
        "Perfetto! Continua così!"
    };

    public event Action OnContinueButtonPressed;

    private void Awake()
    {
        // Set up continue button listener
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(HandleContinueButton);
        }

        // Ensure final score canvas is hidden at start
        if (finalScoreCanvas != null)
        {
            finalScoreCanvas.SetActive(false);
        }

        // Hide continue button initially
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }

        // Ensure compliment text is initially hidden
        if (complimentText != null)
        {
            complimentText.gameObject.SetActive(false);
        }
    }

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

    // Displays the final score with the number of completed tasks
    public void DisplayFinalScore(int completedTasks)
    {
        scoreText.text = $"Complimenti! Hai completato corrrettamente {completedTasks} operazioni!";
    }

    // Displays the endgame message when the game is completed
    public void DisplayEndGameMessage()
    {
        resultText.text = "Complimenti! Hai completato tutte le operazioni correttamente!\r\n\r\nPremi su Esci oppure ricomincia il gioco!";
    }

    // Displays whether the player wins or loses a round
    public void DisplayResult(bool isCorrect)
    {
        if (isCorrect)
        {
            // Hide result text
            resultText.gameObject.SetActive(false);

            // Show a random compliment message
            if (complimentText != null)
            {
                complimentText.text = complimentMessages[UnityEngine.Random.Range(0, complimentMessages.Length)];
                complimentText.gameObject.SetActive(true);
            }

            // Play win sound
            if (audioSource != null && winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }

            // Show continue button
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // Random lose message
            resultText.text = loseMessages[UnityEngine.Random.Range(0, loseMessages.Length)];

            // Play lose sound
            if (audioSource != null && loseSound != null)
            {
                audioSource.PlayOneShot(loseSound);
            }
        }
    }

    // Show final score canvas and hide game canvas
    public void ShowFinalScoreCanvas()
    {
        if (gameCanvas != null)
        {
            gameCanvas.SetActive(false);
            if (audioSource != null && endGameSound != null)
            {
                audioSource.PlayOneShot(endGameSound, 1);
            }
            gameButtonCanvas.SetActive(false);
        }

        if (finalScoreCanvas != null)
        {
            finalScoreCanvas.SetActive(true);
            finalScoreButtonCanvas.SetActive(true);
        }



        // Set final result text
        if (finalResultText != null && resultText != null)
        {
            finalResultText.text = resultText.text;
        }
    }

    // Prepare for next operation
    public void PrepareForNextOperation()
    {
        // Hide compliment text and show resultText
        if (complimentText != null)
        {
            complimentText.gameObject.SetActive(false);

        }

        // Show result text again
        resultText.gameObject.SetActive(true);
        resultText.text = "Pescando le carte, risolvi l'operazione qui sopra posizionandola in colonna\r\n\r\nQuando sei sicuro, premi su VERIFICA"; // Clear previous message

        // Hide continue button
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
        // Play newRound Clip
        if (audioSource != null && newRoundSound != null)
        {
            audioSource.PlayOneShot(newRoundSound, 1);
        }
    }

    // Handle continue button press
    private void HandleContinueButton()
    {
        // Invoke the event to notify the GameManager
        OnContinueButtonPressed?.Invoke();
    }
}