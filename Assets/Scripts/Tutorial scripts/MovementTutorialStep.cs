using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using System.Collections.Generic;

public class MovementTutorialStep : MonoBehaviour, ITutorialStep
{
    [Header("UI References")]
    public Canvas movementTutorialCanvas;
    public Button continueButton;
    public TextMeshProUGUI instructionText;

    [Header("References")]
    public Transform teleportAnchor;
    public XROrigin xrRig; // Reference to the XR Rig

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

    private TutorialManager tutorialManager;

    public void StartStep(TutorialManager manager)
    {
        tutorialManager = manager;

        // Teleport the player
        TeleportPlayer();

        // Enable the canvas and show instructions
        movementTutorialCanvas.enabled = true;
        ShowMovementInstructions();

        // Add listener for the continue button
        continueButton.onClick.AddListener(OnContinueButtonClick);

        // Find controller references
        leftControllerHighlight = null;
        rightControllerHighlight = null;

        // Highlight the controller buttons
        HighlightController();
    }

    public void EndStep()
    {
        // Hide canvas and cleanup listeners
        movementTutorialCanvas.enabled = false;
        continueButton.onClick.RemoveListener(OnContinueButtonClick);
        ResetHighlights();
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

    private void TeleportPlayer()
    {
        if (teleportAnchor != null && xrRig != null)
        {
            Vector3 heightAdjustedPosition = teleportAnchor.position;
            heightAdjustedPosition.y = xrRig.transform.position.y; // Maintain current height
            xrRig.transform.position = heightAdjustedPosition;
        }
    }

    private void ShowMovementInstructions()
    {
        instructionText.text = "Ora impariamo come muoverci:\n\n" +
            "1. Punta il controller sul pavimento, vedrai un arco <color=#0fd1cb>Azzurro</color>\n" +
            "2. Scegli un punto dove vuoi <color=#0fd1cb>teletrasportarti</color>\n" +
            "3. Premi il <color=#0fd1cb>grilletto lampeggiante</color> sotto il tuo dito medio per teletrasportati nel punto indicato\n\n" +
            "Fai pratica con il movimento. Quando ti senti pronto, avvicinati alla lavagna premi su continua per proseguire il tutorial.";
    }

    private void OnContinueButtonClick()
    {
        // Notify the TutorialManager to proceed to the next step
        tutorialManager.ProceedToNextStep();

        // Play sound to indicate the step is complete
        soundEffects?.PlayNewRoundSound();
    }
}
