using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotManager_CompleteOperation : MonoBehaviour
{
    public List<XRSocketInteractor> cardSlots;  // List of XR Sockets (the slots where cards are placed)
    public List<SlotController> slotControllers;  // List of Slot Controllers (to manage slot highlights)

    // Highlights a slot at index i as incorrect
    public void HighlightIncorrectSlot(int index)
    {
        slotControllers[index].SetIncorrect(true);
    }

    // Clears the highlight for a slot at index i
    public void ClearHighlightSlot(int index)
    {
        slotControllers[index].SetIncorrect(false);
    }

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
    public void ClearAllSlots()
    {
        foreach (XRSocketInteractor slot in cardSlots)
        {
            if (slot.hasSelection)
            {
                // Get the selected interactable (card) and destroy it
                var selectedInteractable = slot.GetOldestInteractableSelected();
                Destroy(selectedInteractable.transform.gameObject);
            }
        }
    }

}
