using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Unity.XR.CoreUtils;

public class SocketTutorialStep : MonoBehaviour
{
    [Header("UI References")]
    public Canvas socketTutorialCanvas;
    public TextMeshProUGUI instructionText;
    public Canvas endTutorialCanvas;

    [Header("Scene References")]
    public Transform playerAnchor;
    public Transform setupSpawnPoint;
    public XROrigin xrRig;
    public GrabbingTutorialStep previousStep;
    public XRSocketInteractor keySocket;

    [Header("Prefab")]
    public GameObject tableWithKeyPrefab;

    [Header("References")]
    public TutorialSoundEffects soundEffects;

    private GameObject spawnedSetup;

    public event System.Action OnStepComplete;

    private void Start()
    {
        // Initially hide canvases
        socketTutorialCanvas.enabled = false;
        endTutorialCanvas.enabled = false;

        if (previousStep != null)
        {
            previousStep.OnStepComplete += StartSocketTutorial;
        }

        // Listen for key socket interactions
        if (keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnKeyInserted);
        }
    }

    private void StartSocketTutorial()
    {
        TeleportPlayer();
        SpawnTableSetup();
        ShowSocketInstructions();
        socketTutorialCanvas.enabled = true;
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
            "1. Ad esempio, trova la chiave nascosta e raccoglila\n" +
            "2. Una volta presa in mano, cerca la serratura sulla porta\n" +
            "3. Inserisci la chiave e poi ritorna di fronte alla lavagna!\n\n" +
            "Completa questi passaggi per terminare il tutorial!";
    }

    private void OnKeyInserted(SelectEnterEventArgs args)
    {
        // Check if the inserted object is indeed the key
        if (args.interactableObject.transform.CompareTag("Key"))
        {
            // Play win sound
            soundEffects.PlayWinSound();

            ShowEndTutorial();
        }
    }

    private void ShowEndTutorial()
    {
        // Hide socket tutorial canvas
        socketTutorialCanvas.enabled = false;

        // Show end tutorial canvas
        endTutorialCanvas.enabled = true;

        // Play end tutorial sound
        soundEffects.PlayEndGameSound();

        // You might want to trigger any end tutorial effects here
        OnStepComplete?.Invoke();
    }

    private void OnDestroy()
    {
        if (previousStep != null)
        {
            previousStep.OnStepComplete -= StartSocketTutorial;
        }

        if (keySocket != null)
        {
            keySocket.selectEntered.RemoveListener(OnKeyInserted);
        }

        if (spawnedSetup != null)
        {
            Destroy(spawnedSetup);
        }
    }
}