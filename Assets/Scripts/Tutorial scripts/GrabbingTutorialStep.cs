using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using System.Collections.Generic;

public class GrabbingTutorialStep : MonoBehaviour, ITutorialStep
{
    [Header("UI References")]
    public Canvas grabbingTutorialCanvas;
    public Button continueButton;
    public TextMeshProUGUI instructionText;

    [Header("Scene References")]
    public Transform playerAnchor;
    public Transform tableSpawnPoint;
    public XROrigin xrRig;

    [Header("Prefab")]
    public GameObject tableWithObjectsPrefab;

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

    private GameObject spawnedSetup;
    private TutorialManager tutorialManager;

    public void StartStep(TutorialManager manager)
    {
        tutorialManager = manager;

        // Enable the canvas for this step
        grabbingTutorialCanvas.enabled = true;

        soundEffects?.PlayNewRoundSound();
        // Play audio instructions
        soundEffects?.PlayGrabbingInstructions();

        // Teleport the player and set up the environment
        TeleportPlayer();
        SpawnTableSetup();

        // Show instructions
        ShowGrabbingInstructions();

        // Add button listener
        continueButton.onClick.AddListener(OnContinueButtonClick);

        // Find controller references
        leftControllerHighlight = null;
        rightControllerHighlight = null;

        // Highlight the controller buttons
        HighlightController();
    }

    public void EndStep()
    {
        // Hide the canvas
        grabbingTutorialCanvas.enabled = false;

        // Remove button listener
        continueButton.onClick.RemoveListener(OnContinueButtonClick);

        // Cleanup the spawned setup
        if (spawnedSetup != null)
        {
            Destroy(spawnedSetup);
        }
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
        if (playerAnchor != null && xrRig != null)
        {
            Vector3 heightAdjustedPosition = playerAnchor.position;
            heightAdjustedPosition.y = xrRig.transform.position.y;
            xrRig.transform.position = heightAdjustedPosition;
            xrRig.transform.rotation = playerAnchor.rotation;
        }
    }

    private void SpawnTableSetup()
    {
        if (tableWithObjectsPrefab != null && tableSpawnPoint != null)
        {
            spawnedSetup = Instantiate(tableWithObjectsPrefab,
                                        tableSpawnPoint.position,
                                        tableSpawnPoint.rotation);
        }
    }

    private void ShowGrabbingInstructions()
    {
        instructionText.text = "Puoi anche afferrare gli oggetti!\n\n" +
            "1. Punta il controller su un oggetto, vedrai il raggio <color=#0fd1cb>Azzurro</color>\n" +
            "2. Tieni premuto il <color=#0fd1cb>grilletto lampeggiante</color> sotto il tuo dito medio per afferrarlo\n" +
            "3. Ora puoi usare le <color=#0fd1cb>levette lampeggianti</color> per ruotarlo e muoverlo avanti e indietro!\n\n" +
            "Puoi anche avvicinarti con la mano e afferrarlo come se fosse reale.\n" +
            "Premi continua per proseguire.";
    }

    private void OnContinueButtonClick()
    {
        tutorialManager.ProceedToNextStep();
        soundEffects.PlayNewRoundSound();
    }
}
