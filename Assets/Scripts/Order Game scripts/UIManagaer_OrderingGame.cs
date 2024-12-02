using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_OrderingGame : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshPro feedbackText;
    public TextMeshPro difficultyText;
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
        feedbackText.text = $"Ordina i cubi in modo {sortingOrder} posizionandoli sui piedistalli gialli.";
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

    public void HandleCorrectSolution(int currentTurn, int totalTurns)
    {
        // Play correct order audio
        audioSource.PlayOneShot(correctOrderClip, 1);

        // Hide check solution button and show continue button
        checkSolutionButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);

        // Update feedback text
        feedbackText.text = $"Bravo! Hai completato il turno {currentTurn}/{totalTurns}. Premi Continua!";
    }

    public void HandleIncorrectSolution(bool isAscendingOrder)
    {

        string sortingOrder = isAscendingOrder ? "Crescente" : "Decrescente";

        // Play incorrect order audio
        audioSource.PlayOneShot(incorrectOrderClip, 1);

        // Update feedback text with encouragement
        feedbackText.text = $"Accidenti! Alcuni blocchi sono fuori posto. Riposiziona quelli in rosso! Ricordati di ordinarli in modo {sortingOrder}";
    }

    public void ShowFinalResultCanvas()
    {
        gameplayCanvas.SetActive(false);
        finalResultCanvas.SetActive(true);
    }

    public void ResetTurnUI()
    {
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