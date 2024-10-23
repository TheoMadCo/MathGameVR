using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotManager_CompleteOperation : MonoBehaviour
{
    public List<XRSocketInteractor> cardSlots;  // List of XR Sockets (the slots where cards are placed)

    // Get the numbers placed in the slots, considering empty slots as nulls
    public List<int?> GetPlacedCardNumbers()
    {
        List<int?> cardNumbers = new List<int?>();

        foreach (XRSocketInteractor slot in cardSlots)
        {
            if (slot.hasSelection)
            {
                // Get the card number from the card placed in the slot
                var card = slot.GetOldestInteractableSelected().transform.GetComponent<CardController>();
                cardNumbers.Add(card.GetNumber());  // Add the number to the list
            }
            else
            {
                cardNumbers.Add(null);  // If no card is placed, treat it as null (empty)
            }
        }
        return cardNumbers;
    }
}
