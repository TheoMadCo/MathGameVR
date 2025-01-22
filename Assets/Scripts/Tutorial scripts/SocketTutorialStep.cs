using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections.Generic;

public class SocketTutorialStep : MonoBehaviour, ITutorialStep
{
    [Header("UI References")]
    public Canvas socketTutorialCanvas;
    public TextMeshProUGUI instructionText;
    public Canvas endTutorialCanvas;

    [Header("Scene References")]
    public Transform playerAnchor;
    public Transform setupSpawnPoint;
    public XROrigin xrRig;
    public XRSocketInteractor keySocket;
    public UnityEngine.UI.Button resetButton; 

    [Header("Prefab")]
    public GameObject tableWithKeyPrefab;

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

        // Teleport the player and set up the environment
        TeleportPlayer();
        SpawnTableSetup();

        // Show instructions and enable canvas
        ShowSocketInstructions();
        socketTutorialCanvas.enabled = true;

        // Listen for key socket interactions
        if (keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnKeyInserted);
        }

        // Wire up the reset button
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetTableSetup);
        }

        // Find controller references
        leftControllerHighlight = null;
        rightControllerHighlight = null;

        // Highlight the controller buttons
        HighlightController();
    }

    public void EndStep()
    {
        // Cleanup and disable canvases
        socketTutorialCanvas.enabled = false;
        endTutorialCanvas.enabled = false;

        if (keySocket != null)
        {
            keySocket.selectEntered.RemoveListener(OnKeyInserted);
        }
        ResetHighlights();

        // Clean up the reset button listener
        if (resetButton != null)
        {
            resetButton.onClick.RemoveListener(ResetTableSetup);
        }
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
            heightAdjustedPosition.y = xrRig.transform.position.y; // Maintain player height
            xrRig.transform.position = heightAdjustedPosition;
            xrRig.transform.rotation = playerAnchor.rotation;
        }
    }

    private void SpawnTableSetup()
    {
        if (tableWithKeyPrefab != null && setupSpawnPoint != null)
        {
            spawnedSetup = Instantiate(tableWithKeyPrefab,
                                        setupSpawnPoint.position,
                                        setupSpawnPoint.rotation);
        }
    }

    private void ShowSocketInstructions()
    {
        instructionText.text = "Puoi anche posizionare gli oggetti in degli appositi slot:\n\n" +
            "1. Ad esempio, <color=#0fd1cb>trova la chiave</color> nascosta e raccoglila\n" +
            "2. Una volta presa in mano, <color=#0fd1cb>cerca la serratura</color> sulla porta e avvicinati con il <color=#0fd1cb>teletrasporto</color> \n" +
            "3. <color=#0fd1cb>Avvicina la chiave alla serratura</color>, rilascia la presa e vedrai la chiave entrare!\n\n" +
            "Completa questi passaggi per terminare il tutorial!";
    }

    private void OnKeyInserted(SelectEnterEventArgs args)
    {
        // Verify the correct object (key) was inserted
        if (args.interactableObject.transform.CompareTag("Key"))
        {
            // Play success sound
            soundEffects?.PlayWinSound();

            // Notify the manager to proceed to the next step
            tutorialManager.ProceedToNextStep();
        }
    }

    private void ResetTableSetup()
    {
        // Destroy the current setup if it exists
        if (spawnedSetup != null)
        {
            Destroy(spawnedSetup);
        }

        // Spawn a new setup using the existing SpawnTableSetup function
        SpawnTableSetup();

        // Play a sound effect to indicate reset (optional)
        soundEffects?.PlayNewRoundSound();
    }


    private void ShowEndTutorial()
    {
        // Hide the socket tutorial canvas
        socketTutorialCanvas.enabled = false;

        // Show the end tutorial canvas
        endTutorialCanvas.enabled = true;

        // Play end tutorial sound
        soundEffects?.PlayEndGameSound();

        // Notify the TutorialManager to proceed to the next step
        tutorialManager.ProceedToNextStep();
    }
}
