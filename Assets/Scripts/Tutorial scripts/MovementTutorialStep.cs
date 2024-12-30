using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;


public class MovementTutorialStep : MonoBehaviour
{
    [Header("UI References")]
    public Canvas movementTutorialCanvas;
    public Button continueButton;
    public TextMeshProUGUI instructionText;

    [Header("References")]
    public Transform teleportAnchor;
    public UITutorialStep previousStep;
    public XROrigin xrRig; // Reference to the XR Rig

    [Header("References")]
    public TutorialSoundEffects soundEffects;

    public event System.Action OnStepComplete; // Event that will be invoked when the step is completed

    private void Start()
    {

        // Initially hide this step's canvas
        movementTutorialCanvas.enabled = false;

        // Listen to the previous step in order to start this step
        if (previousStep != null)
        {
            previousStep.OnStepComplete += StartMovementTutorial; // Subscribe to the previous step (in this way we can chain the steps)
        }

        continueButton.onClick.AddListener(OnContinueButtonClick); // Subscribe to the button click event
    }

    // This method will be called when the previous step is completed
    private void StartMovementTutorial() 
    {
        // Tteleport the player
        TeleportPlayer();

        // Enable the canvas and show instructions
        movementTutorialCanvas.enabled = true;
        ShowMovementInstructions();
    }

    // Teleport the player to the anchor's position
    private void TeleportPlayer()
    {
        if (teleportAnchor != null && xrRig != null)
        {
            Vector3 heightAdjustedPosition = teleportAnchor.position;
            heightAdjustedPosition.y = xrRig.transform.position.y;
            xrRig.transform.position = heightAdjustedPosition;
        }
    }

    private void ShowMovementInstructions()
    {
        instructionText.text = "Ora impariamo come muoverci:\n\n" +
            "1. Punta il controller sul pavimento, vedrai un arco blu\n" +
            "2. Scegli un punto dove vuoi teletrasportarti\n" +
            "3. Premi il grilletto sotto il tuo dito medio per teletrasportati nel punto indicato\n" +
            "Fai pratica con il movimento. Quando ti senti pronto, avvicinati alla lavagna premi su continua per proseguire il tutorial.";
    }

    private void OnContinueButtonClick()
    {
        OnStepComplete?.Invoke(); // Invoke the event to notify that this step is completed
        movementTutorialCanvas.enabled = false;
        soundEffects.PlayNewRoundSound();
    }

    private void OnDestroy()
    {
        if (previousStep != null)
        {
            previousStep.OnStepComplete -= StartMovementTutorial; // Unsubscribe from the previous step
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClick); // Unsubscribe from the button click event
        }
    }
}