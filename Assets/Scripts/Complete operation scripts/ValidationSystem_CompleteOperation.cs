using System.Collections.Generic;
using UnityEngine;

/* JUSTTO HELP THE UNDERSTANDING OF THE CODE
 * ValidationSystem_CompleteOperation
 * -----------------------------------
 * This script handles the validation logic. It compares the player's input (placed cards)
 * against the expected numbers for the given operation (addition, subtraction, multiplication) to determine if the solution is correct.
 * 
 * Core Responsibilities:
 * 1. Retrieve the player's input from the active slots (via `SlotManager_CompleteOperation`).
 * 2. Generate the expected digits for the operation, considering:
 *    - The left operand (e.g., the first number in "15 + 3").
 *    - The right operand (e.g., the second number in "15 + 3").
 *    - The result (e.g., "18" for "15 + 3").
 * 3. Highlight incorrect slots and provide feedback on validation.
 * 4. Ensure that slots marked as disabled (based on difficulty) are treated as `null` during validation.
 * 
 * Key Methods:
 * - ValidateResult():
 *   This is the main method that validates the player's input against the expected solution. It iterates over all slots,
 *   comparing the placed numbers with the expected numbers. Incorrect slots are visually highlighted, and a boolean
 *   result is returned to indicate success or failure.
 * 
 * - GetPlacedCardNumbers():
 *   This method retrieves the numbers currently placed in the active slots. Disabled slots are automatically treated as `null`.
 *   It ensures alignment between player input and slot configuration.
 * 
 * - GetExpectedNumbers():
 *   This method calculates the expected numbers for the current operation, splitting each number into its digits. It ensures
 *   numbers are padded with `null` to match the slot layout. Special handling is included to ensure `0` is treated as a valid
 *   digit and not as an empty slot.
 * 
 * Debugging:
 * - Debug logs are included to output the player's placed numbers, expected numbers, and slot validation results.
 * - These logs are useful for identifying misalignments between input and expected values.
 * 
 * Dependencies:
 * - `SlotManager_CompleteOperation`: Manages the state of slots (active/inactive) and provides the player's input for validation.
 * - `OperationGenerator`: Provides the current operation details (operands, operator type, and result).
 * 
 * How It Works:
 * 1. The script retrieves the player's input and expected numbers.
 * 2. It iterates over all slots, comparing the player's input with the expected solution.
 * 3. Slots are marked as correct or incorrect, and a boolean value is returned to indicate the result.
 * 
 * 
 */

public class ValidationSystem_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public SlotManager_CompleteOperation slotManager;
    public List<int> correctSlotIndices = new List<int>();  // Track correct slot indices

    // Validates the placed card numbers against the expected numbers
    public bool ValidateResult()
    {
        List<int?> placedNumbers = slotManager.GetPlacedCardNumbers();  // Includes only active slots
        List<int?> expectedNumbers = GetExpectedNumbers();

        Debug.Log($"Placed Numbers: {string.Join(", ", placedNumbers)}");
        Debug.Log($"Expected Numbers: {string.Join(", ", expectedNumbers)}");

        bool isAllCorrect = true;  // Track if all numbers are correct
        correctSlotIndices.Clear();  // Clear previous correct slot indices

        // Compare each slot, marking incorrect or correct
        for (int i = 0; i < expectedNumbers.Count; i++)
        {
            int? expectedDigit = expectedNumbers[i];
            int? placedDigit = placedNumbers[i];

            if (expectedDigit != placedDigit)
            {
                isAllCorrect = false;  // At least one digit is incorrect

                if (slotManager.slotControllers[i].IsActive())  // Highlight only active slots
                {
                    Debug.Log($"Slot {i} is incorrect. Expected: {expectedDigit}, Placed: {placedDigit}");
                    slotManager.HighlightIncorrectSlot(i);  // Highlight incorrect slot
                }
            }
            else
            {
                Debug.Log($"Slot {i} is correct. Value: {expectedDigit}");

                if (slotManager.slotControllers[i].IsActive())  // Only track active slots
                {
                    slotManager.HighlightCorrectSlot(i);  // Highlight correct slot in green
                    correctSlotIndices.Add(i);  // Add index of correct slot
                }
            }
        }

        Debug.Log(isAllCorrect
            ? "Validation successful! All numbers are correct."
            : "Validation failed. Some numbers are incorrect.");

        return isAllCorrect;  // Return true if all numbers are correct
    }

    // Generate the expected numbers based on the operation type and difficulty
    private List<int?> GetExpectedNumbers()
    {
        List<int?> expectedNumbers = new List<int?>();  // Declare the variable at the start

        // Split a number into its digits
        List<int?> SplitNumberIntoDigits(int number, int maxDigits)
        {
            List<int?> digits = new List<int?>();

            if (number == 0)
            {
                // Handle the case where the entire number is 0
                digits.Add(0);
            }
            else
            {
                // Extract digits for non-zero numbers
                while (number > 0)
                {
                    digits.Add(number % 10);  // Get the least significant digit
                    number /= 10;  // Remove the least significant digit
                }
            }

            // Pad with nulls to align with maxDigits
            while (digits.Count < maxDigits)
            {
                digits.Add(null);
            }

            digits.Reverse();  // Reverse to align with left-to-right column layout
            return digits;
        }

        int maxDigits = 3;  // For a 3x3 grid

        // Generate expected numbers for left operand
        expectedNumbers.AddRange(SplitNumberIntoDigits(operationGenerator.LeftOperand, maxDigits));

        // Generate expected numbers for right operand
        expectedNumbers.AddRange(SplitNumberIntoDigits(operationGenerator.RightOperand, maxDigits));

        // Generate expected numbers for the result
        int result = operationGenerator.operationType switch
        {
            OperationType.Addition => operationGenerator.LeftOperand + operationGenerator.RightOperand,
            OperationType.Subtraction => operationGenerator.LeftOperand - operationGenerator.RightOperand,
            OperationType.Multiplication => operationGenerator.LeftOperand * operationGenerator.RightOperand,
            _ => 0
        };

        expectedNumbers.AddRange(SplitNumberIntoDigits(result, maxDigits));

        Debug.Log($"Expected Numbers: {string.Join(", ", expectedNumbers)}");
        return expectedNumbers;
    }
}