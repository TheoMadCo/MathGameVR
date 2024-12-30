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

    // Highlights a slot at index i as correct
    public void HighlightCorrectSlot(int index)
    {
        slotControllers[index].SetCorrect(true);
    }

    // Clears the highlight for a slot at index i
    public void ClearHighlightSlot(int index)
    {
        slotControllers[index].SetIncorrect(false);
        slotControllers[index].SetCorrect(false);
    }

    // Clear all highlights
    public void ClearAllHighlights()
    {
        for (int i = 0; i < slotControllers.Count; i++)
        {
            ClearHighlightSlot(i);
        }
    }

    // Get the numbers placed in the slots, considering empty slots as nulls
    public List<int?> GetPlacedCardNumbers()
    {
        List<int?> placedNumbers = new List<int?>();

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (slotControllers[i].IsActive())  // Only check active slots
            {
                if (cardSlots[i].hasSelection)
                {
                    // Get the card number from the card placed in the slot
                    var card = cardSlots[i].GetOldestInteractableSelected().transform.GetComponent<CardController>();
                    placedNumbers.Add(card.GetNumber());
                }
                else
                {
                    // If the slot is empty, clear its highlight to the default material
                    ClearHighlightSlot(i);
                    placedNumbers.Add(null);  // Treat empty active slots as null
                }
            }
            else
            {
                placedNumbers.Add(null);  // Disabled slots are treated as null
            }
        }

        Debug.Log($"Placed Numbers: {string.Join(", ", placedNumbers)}");
        return placedNumbers;
    }

    // Clears all cards from the slots
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

        // Clear all highlights after clearing slots
        ClearAllHighlights();
    }

    // Configures slots based on difficulty, enabling only the necessary ones
    public void ConfigureSlotsForDifficulty(Difficulty difficulty)
    {
        // Number of active columns (rightmost columns are active)
        int activeColumns = difficulty == Difficulty.Hard ? 3 : 2;  // 3 for Hard, 2 for Easy/Medium

        // Disable sockets in the leftmost columns
        for (int i = 0; i < cardSlots.Count; i++)
        {
            // Determine column index (0 = leftmost, 2 = rightmost in a 3-column grid)
            int column = i % 3;

            // Enable or disable slot based on the active columns
            bool isActive = column >= (3 - activeColumns);  // Rightmost columns are active
            slotControllers[i].SetSlotActive(isActive);
        }

        Debug.Log($"Configured slots for {difficulty} difficulty. Active columns: {activeColumns}");
    }
}