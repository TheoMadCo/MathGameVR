using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class CardStackManager : MonoBehaviour
{
    public GameObject[] numberCardPrefabs;  // Array to hold card prefabs (0-9)

    // Array to hold stacks manually assigned in the Inspector
    public XRBaseInteractable[] cardStacks;  // Array of stacks (assigned in Inspector)

    // Start is called before the first frame update
    void Start()
    {
        // Assign interaction listeners to the stacks directly
        for (int i = 0; i < cardStacks.Length; i++)
        {
            // Assign an interaction listener to each stack
            cardStacks[i].selectEntered.AddListener(OnStackInteracted);
        }
    }

    // This function will be called when a stack is interacted with
    private void OnStackInteracted(SelectEnterEventArgs args)
    {
        // Get the interactor (the hand/controller that interacted)
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;

        // Find the corresponding card number from the stack
        int cardNumber = System.Array.IndexOf(cardStacks, args.interactableObject);

        // If a valid card number is found, spawn the card
        if (cardNumber >= 0 && cardNumber < numberCardPrefabs.Length)
        {
            SpawnCard(cardNumber, interactor);
        }
    }

    // Spawn the card and automatically attach it to the interacting hand (interactor)
    private void SpawnCard(int cardNumber, XRBaseInteractor interactor)
    {
        if (interactor != null && numberCardPrefabs[cardNumber] != null)
        {
            // Instantiate the card at the hand's position (the hand that interacted with the stack)
            GameObject newCard = Instantiate(numberCardPrefabs[cardNumber], interactor.transform.position, interactor.transform.rotation);

            // Auto-attach to the player's hand (the interactor that interacted)
            XRGrabInteractable grabInteractable = newCard.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                // Force the hand (interactor) to grab the card automatically
                interactor.interactionManager.SelectEnter(interactor as IXRSelectInteractor, grabInteractable as IXRSelectInteractable);
            }
        }
    }
}
