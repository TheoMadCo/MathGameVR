using System.Collections.Generic;
using UnityEngine;

public class ValidationSystem_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public SlotManager_CompleteOperation slotManager;

    // Validates the player's input by checking the numbers in the slots
    public bool ValidateResult()
    {
        List<int?> placedNumbers = slotManager.GetPlacedCardNumbers();  // Get the placed card numbers
        List<int?> expectedNumbers = new List<int?>();

        // Add the digits of the left operand
        if (operationGenerator.LeftOperand >= 10)
        {
            expectedNumbers.Add(operationGenerator.LeftOperand / 10);  // Tens place
            expectedNumbers.Add(operationGenerator.LeftOperand % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Single digit, leave the tens place empty
            expectedNumbers.Add(operationGenerator.LeftOperand);  // Ones place filled
        }

        // Add the digits of the right operand
        if (operationGenerator.RightOperand >= 10)
        {
            expectedNumbers.Add(operationGenerator.RightOperand / 10);  // Tens place
            expectedNumbers.Add(operationGenerator.RightOperand % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Single digit, leave the tens place empty
            expectedNumbers.Add(operationGenerator.RightOperand);  // Ones place filled
        }

        // Add the digits of the result
        if (operationGenerator.Result >= 10)
        {
            expectedNumbers.Add(operationGenerator.Result / 10);  // Tens place
            expectedNumbers.Add(operationGenerator.Result % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Single digit, leave the tens place empty
            expectedNumbers.Add(operationGenerator.Result);  // Ones place filled
        }

        // Compare the placed numbers to the expected numbers
        for (int i = 0; i < expectedNumbers.Count; i += 2)  // Iterate over pairs (tens and ones)
        {
            int? expectedTens = expectedNumbers[i];
            int? expectedOnes = expectedNumbers[i + 1];
            int? placedTens = placedNumbers[i];
            int? placedOnes = placedNumbers[i + 1];

            // Handle the case for two-digit numbers (both tens and ones must be filled)
            if (expectedTens != null && expectedOnes != null)
            {
                // If either the tens or ones place is missing for a two-digit number, fail
                if (placedTens == null || placedOnes == null || placedTens != expectedTens || placedOnes != expectedOnes)
                {
                    return false;
                }
            }

            // Handle the case for single-digit numbers (either tens or ones can be filled, but not both empty)
            else if (expectedTens == null && expectedOnes != null)  // Single-digit case
            {
                if ((placedTens != null && placedOnes == null && placedTens == expectedOnes) ||  // Tens filled, ones empty
                    (placedTens == null && placedOnes != null && placedOnes == expectedOnes))  // Ones filled, tens empty
                {
                    continue;  // Valid case for single-digit numbers
                }
                else
                {
                    return false;  // Invalid if neither match or both are empty
                }
            }
        }

        return true;  // All numbers are correct
    }
}
