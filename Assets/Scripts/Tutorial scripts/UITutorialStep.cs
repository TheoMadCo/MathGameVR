using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialStep : MonoBehaviour, ITutorialStep
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

    [System.Serializable]
    public class ButtonHighlightInfo
    {
        public string buttonName;
        public Material highlightMaterial;
        public bool leftController = true;
        public bool rightController = true;
    }

    [Tooltip("Controller highlight configurations for this tutorial step")]
    public List<ButtonHighlightInfo> buttonHighlights = new List<ButtonHighlightInfo>();
    private ControllerButtonHighlight leftControllerHighlight;
    private ControllerButtonHighlight rightControllerHighlight;
    private bool isHighlighted = false;

    private int clickCount = 0;
    private const int REQUIRED_CLICKS = 2;
    private Image practiceButtonImage;

    private TutorialManager tutorialManager;

    public void StartStep(TutorialManager manager)
    {
        tutorialManager = manager;
        soundEffects?.PlayNewRoundSound();

        practiceButtonImage = practiceButton.GetComponent<Image>();

        // Setup initial state
        tutorialCanvas.enabled = true;
        continueButton.gameObject.SetActive(false);
        instructionText.text = "In questo Tutorial imparerai a muoverti nel gioco\r\n\r\nLa prima cosa da imparare è come usare i bottoni. \r\nPunta il bottone con il raggio colorato che esce dalla tua mano, e quando diventerà blu, usa il grilletto sotto il tuo indice per cliccarlo.";

        practiceButton.onClick.AddListener(OnPracticeButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);

        SetRandomButtonColor();

        // Find controller references
        leftControllerHighlight = null;
        rightControllerHighlight = null;

    }

    //Necessary at the first step to allow the objects the time to be instantiated
    private void Update()
    {
        HighlightController();
    }

    public void EndStep()
    {
        ResetHighlights();

        tutorialCanvas.enabled = false;
        practiceButton.onClick.RemoveListener(OnPracticeButtonClick);
        continueButton.onClick.RemoveListener(OnContinueButtonClick);
    }

    private void HighlightController()
    {
        if (leftControllerHighlight == null)
        {
            var leftControllerFinded = GameObject.Find("XRControllerLeftwithHighLights(Clone)");
            if (leftControllerFinded != null)
            {
                leftControllerHighlight = leftControllerFinded.GetComponent<ControllerButtonHighlight>();
            }
        }
        if (rightControllerHighlight == null)
        {
            var rightControllerFinded = GameObject.Find("XRControllerRightwithHighLights(Clone)");
            if (rightControllerFinded != null)
            {
                rightControllerHighlight = rightControllerFinded.GetComponent<ControllerButtonHighlight>();
            }
        }
        if (!isHighlighted && rightControllerHighlight && leftControllerHighlight)
        {
            ApplyHighlights();
            isHighlighted = true;
        }
    }

    private void ApplyHighlights()
    {
        foreach (var highlight in buttonHighlights)
        {
            if (highlight.leftController && leftControllerHighlight != null)
            {
                leftControllerHighlight.HighlightButton(highlight.buttonName, highlight.highlightMaterial);
            }
            if (highlight.rightController && rightControllerHighlight != null)
            {
                rightControllerHighlight.HighlightButton(highlight.buttonName, highlight.highlightMaterial);
            }
        }
    }

    private void ResetHighlights()
    {
        leftControllerHighlight?.ResetAllHighlights();
        rightControllerHighlight?.ResetAllHighlights();
    }

    private void OnPracticeButtonClick()
    {
        clickCount++;
        SetRandomButtonColor();

        if (clickCount >= REQUIRED_CLICKS)
        {
            EnableContinueButton();
        }
        else
        {
            instructionText.text = "Ottimo! Ora clicca ancora il bottone per cambiare di nuovo colore.";
        }
    }

    private void EnableContinueButton()
    {
        continueButton.gameObject.SetActive(true);
        instructionText.text = "Fantastico! Hai imparato come usare i bottoni.\r\n\r\n Continua a cambiare colore e quando vuoi premi su continua per proseguire il tutorial.";
    }

    private void OnContinueButtonClick()
    {
        tutorialManager.ProceedToNextStep();
    }

    private void SetRandomButtonColor()
    {
        int randomIndex = Random.Range(0, buttonColors.Length);
        practiceButtonImage.color = buttonColors[randomIndex];
    }
}
