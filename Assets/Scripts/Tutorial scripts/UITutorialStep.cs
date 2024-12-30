using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialStep : MonoBehaviour
{
    [Header("UI References")]
    public Canvas tutorialCanvas;
    public Button practiceButton;
    public Button continueButton;
    public TextMeshProUGUI instructionText;

    [Header("Settings")]
    [SerializeField]
    private Color[] buttonColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };

    [Header("References")]
    public TutorialSoundEffects soundEffects;

    private int clickCount = 0;
    private const int REQUIRED_CLICKS = 2;
    private Image practiceButtonImage;

    public event System.Action OnStepComplete; // Event that will be invoked when the step is completed

    private void Start()
    {
        // Play new round sound
        soundEffects.PlayNewRoundSound();

        practiceButtonImage = practiceButton.GetComponent<Image>();

        // Setup initial state
        tutorialCanvas.enabled = true;
        continueButton.gameObject.SetActive(false);
        instructionText.text = "In questo Tutorial imparerai a muoverti nel gioco\r\n\r\nLa prima cosa da imparare è come usare i bottoni. \r\nPunta il bottone con il raggio colorato che esce dalla tua mano, e quando diventerà blu, usa il grilletto sotto il tuo indice per cliccarlo.";

        practiceButton.onClick.AddListener(OnPracticeButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);

        SetRandomButtonColor();
    }

    private void OnPracticeButtonClick()
    {
        clickCount++;
        SetRandomButtonColor();

        if (clickCount >= REQUIRED_CLICKS)
        {
            EnableContinueButton();
        }
        else if (clickCount <= REQUIRED_CLICKS) {
            instructionText.text = "Ottimo! Ora clicca ancora il bottone per cambiare di nuovo colore";
        }
    }

    private void EnableContinueButton()
    {
        continueButton.gameObject.SetActive(true);
        instructionText.text = "Fantastico! Hai imparato come usare i bottni. \r\n\r\n Continua a cambiare colore e quando vuoi premi su continua per proseguire il tutorial";
    }

    private void OnContinueButtonClick()
    {
        OnStepComplete?.Invoke(); // Invoke the event to notify that the step is completed
        tutorialCanvas.enabled = false; // Hide the canvas
        // Play new round sound
        soundEffects.PlayNewRoundSound();
    }

    private void SetRandomButtonColor()
    {
        int randomIndex = Random.Range(0, buttonColors.Length);
        practiceButtonImage.color = buttonColors[randomIndex];
    }

    private void OnDestroy()
    {
        practiceButton.onClick.RemoveListener(OnPracticeButtonClick); // Unsubscribe from the button click event
        continueButton.onClick.RemoveListener(OnContinueButtonClick); // Unsubscribe from the button click event
    }   
}