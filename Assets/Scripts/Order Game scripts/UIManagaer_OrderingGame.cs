using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_OrderingGame : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshPro feedbackText;
    public TextMeshPro difficultyText;
    public TextMeshPro finalResultText;
    public Button checkSolutionButton;
    public Button continueButton;
    public GameObject gameplayCanvas;
    public GameObject finalResultCanvas;

    [Header("Materials")]
    public Material correctMaterial;
    public Material incorrectMaterial;
    public Material defaultMaterial;

    [Header("Button Colors")]
    public Color enabledColor = Color.green;
    public Color disabledColor = Color.grey;

    [Header("Audio")]
    public AudioClip correctOrderClip;
    public AudioClip incorrectOrderClip;
    public AudioClip endGameClip;
    public AudioClip newRoundClip;

    private AudioSource audioSource;
    private GameManager_OrderingGame gameManager;

    void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Find the OrderGameManager in the scene
        gameManager = FindObjectOfType<GameManager_OrderingGame>();
    }

    public void UpdateFeedbackText(string difficulty, int currentTurn, int totalTurns, bool isAscendingOrder)
    {
        string sortingOrder = isAscendingOrder ? "Crescente" : "Decrescente";
        string currentDifficulty = difficulty == "Easy" ? "Facile" : difficulty == "Medium" ? "Media" : "Difficile"; // Italianized difficulty levels
        feedbackText.text = $"Ordina i cubi in modo <color=#0fd1cb>{sortingOrder}</color> posizionandoli sui piedistalli gialli.\n\n";
        difficultyText.text = $"Stai giocando a difficoltà: {currentDifficulty}. Ancora {totalTurns - currentTurn} turni per finire il gioco";
    }

    public void SetButtonInteractable(bool interactable)
    {
        checkSolutionButton.interactable = interactable;

        ColorBlock colors = checkSolutionButton.colors;
        colors.normalColor = interactable ? enabledColor : disabledColor;
        checkSolutionButton.colors = colors;
    }

    public void InitializeGameUI(int totalTurns)
    {
        // Ensure gameplay canvas is active and final result canvas is hidden
        gameplayCanvas.SetActive(true);
        finalResultCanvas.SetActive(false);

        // Hide continue button initially
        continueButton.gameObject.SetActive(false);

        // Disable the "Check Solution" button initially
        SetButtonInteractable(false);
    }

    public void HandleCorrectSolution(int currentTurn, int totalTurns, int pointsEarned, int maxPoints)
    {
        // Play correct order audio
        audioSource.PlayOneShot(correctOrderClip, 1);

        // Hide check solution button and show continue button
        checkSolutionButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);

        // Update feedback text with earned points
        feedbackText.text = $"Bravo! Hai completato il turno {currentTurn} di {totalTurns}.\n Hai guadagnato <color=#0fd1cb>{pointsEarned} punti</color> su {maxPoints}.\n Premi Continua!";
    }


    public void HandleIncorrectSolution(bool isAscendingOrder, int pointsLost, int maxPoints)
    {
        string sortingOrder = isAscendingOrder ? "Crescente" : "Decrescente";

        // Play incorrect order audio
        audioSource.PlayOneShot(incorrectOrderClip, 1);

        // Update feedback text with points lost
        feedbackText.text = $"Accidenti! Alcuni blocchi sono fuori posto. <color=#F96990> Hai perso 1 punto </color>su {maxPoints} possibili.\n Ricordati di ordinarli in modo <color=#0fd1cb>{sortingOrder}</color>";
    }


    public void ShowFinalResultCanvas(int currentScore, int maxPossibleScore)
    {
        gameplayCanvas.SetActive(false);
        finalResultCanvas.SetActive(true);

        if (audioSource != null && endGameClip != null)
        {
            audioSource.PlayOneShot(endGameClip, 1);
        }

        finalResultText.text = $"Complimenti!\n Hai totalizzato <color=#0fd1cb>{currentScore} punti</color> su un massimo di {maxPossibleScore}.\n\nGioca di nuovo oppure Esci dal gico.";
    }


    public void ResetTurnUI()
    {
        audioSource.PlayOneShot(newRoundClip, 1);
        // Reset UI elements
        checkSolutionButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        // Disable the "Check Solution" button initially
        SetButtonInteractable(false);
    }

    // Helper method to color blocks based on their correctness
    public void ColorBlocks(GameObject[] blockObjects, bool[] blockPositions)
    {
        for (int i = 0; i < blockObjects.Length; i++)
        {
            Renderer blockRenderer = blockObjects[i].GetComponent<Renderer>();
            if (blockRenderer != null)
            {
                blockRenderer.material = blockPositions[i] ? correctMaterial : incorrectMaterial;
            }
        }
    }

    // Optional: Revert blocks to default material
    public void ResetBlockMaterials(GameObject[] blockObjects)
    {
        foreach (GameObject block in blockObjects)
        {
            Renderer blockRenderer = block.GetComponent<Renderer>();
            if (blockRenderer != null)
            {
                blockRenderer.material = defaultMaterial;
            }
        }
    }
}