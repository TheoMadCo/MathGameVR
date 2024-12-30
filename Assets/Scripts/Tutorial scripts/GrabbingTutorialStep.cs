using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;



public class GrabbingTutorialStep : MonoBehaviour
{
    [Header("UI References")]
    public Canvas grabbingTutorialCanvas;
    public Button continueButton;
    public TextMeshProUGUI instructionText;

    [Header("Scene References")]
    public Transform playerAnchor;
    public Transform tableSpawnPoint;
    public XROrigin xrRig;
    public MovementTutorialStep previousStep;

    [Header("Prefab")]
    public GameObject tableWithObjectsPrefab; // Single prefab containing table and all objects

    [Header("References")]
    public TutorialSoundEffects soundEffects;

    private GameObject spawnedSetup;

    public event System.Action OnStepComplete;

    private void Start()
    {
        // Initially hide canvas
        grabbingTutorialCanvas.enabled = false;

        // Listen to previous step completion
        if (previousStep != null)
        {
            previousStep.OnStepComplete += StartGrabbingTutorial;
        }

        continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    private void StartGrabbingTutorial()
    {
        // First teleport player
        TeleportPlayer();

        // Then spawn table setup
        SpawnTableSetup();

        // Finally show UI
        ShowGrabbingInstructions();
        grabbingTutorialCanvas.enabled = true;
    }

    private void TeleportPlayer()
    {
        if (playerAnchor != null && xrRig != null)
        {
            Vector3 heightAdjustedPosition = playerAnchor.position;
            heightAdjustedPosition.y = xrRig.transform.position.y;
            xrRig.transform.position = heightAdjustedPosition;

            // Rotate player to face the table
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
            "1. Punta il controller su un oggetto, vedrai la linea blu\n" +
            "2. Tieni premuto il grilletto sotto il tuo dito medio per afferrarlo\n" +
            "3. Ora puoi usare le levette per ruotarlo e muoverlo avanti e indietro!\n\n" +
            "Puoi anche avvicinarti con la mano e afferrarlo come se fosse reale.\n" +
            "Prova liberamente a giocare con gli oggetti, poi premi continua per proseguire.";
    }

    private void OnContinueButtonClick()
    {
        OnStepComplete?.Invoke();
        Destroy(spawnedSetup);
        grabbingTutorialCanvas.enabled = false;
        soundEffects.PlayNewRoundSound();
    }

    private void OnDestroy()
    {
        if (previousStep != null)
        {
            previousStep.OnStepComplete -= StartGrabbingTutorial;
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClick);
        }

        if (spawnedSetup != null)
        {
            Destroy(spawnedSetup);
        }
    }
}