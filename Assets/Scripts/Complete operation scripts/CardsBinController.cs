using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardsBinController : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        // Register the Select Entered event to trigger when a card is attached
        socketInteractor.selectEntered.AddListener(OnCardAttached);
    }

    private void OnDisable()
    {
        // Unregister the event when the object is disabled or destroyed
        socketInteractor.selectEntered.RemoveListener(OnCardAttached);
    }

    // Called when a card is fully attached to the socket
    private void OnCardAttached(SelectEnterEventArgs args)
    {
        GameObject card = args.interactableObject.transform.gameObject;

        if (card.CompareTag("Card"))
        {
            Debug.Log("Card successfully attached to trash bin: " + card.name);

            // Clear the selection to allow detachment before destroying the card
            socketInteractor.interactablesSelected.Clear();
            Destroy(card);  // Destroy the card after clearing selection
        }
    }
}
