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
        List<int?> expectedNumbers = GetExpectedNumbers();              // Get the expected numbers for the current operation

        Debug.Log("Expected Numbers: " + string.Join(", ", expectedNumbers));  // Log expected numbers
        Debug.Log("Placed Numbers: " + string.Join(", ", placedNumbers));      // Log placed numbers

        // Compare each slot pair (tens and ones) to validate the result
        for (int i = 0; i < expectedNumbers.Count; i++)
        {
            int? expectedDigit = expectedNumbers[i];
            int? placedDigit = placedNumbers[i];

            // Log each comparison for debugging
            Debug.Log($"Comparing slot {i}: Expected = {expectedDigit}, Placed = {placedDigit}");

            // If the expected number and placed number don't match, validation fails
            if (expectedDigit != placedDigit)
            {
                Debug.Log("Mismatch found. Player loses.");
                return false;  // The player loses if any digit is incorrect
            }
        }

        Debug.Log("All numbers match. Player wins!");
        return true;  // All digits are correct
    }

    // Get expected digits for the current operation type
    private List<int?> GetExpectedNumbers()
    {
        List<int?> expectedNumbers = new List<int?>();

        // Left Operand
        if (operationGenerator.LeftOperand >= 10)
        {
            expectedNumbers.Add(operationGenerator.LeftOperand / 10);  // Tens place
            expectedNumbers.Add(operationGenerator.LeftOperand % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Leave tens place empty for single-digit
            expectedNumbers.Add(operationGenerator.LeftOperand);  // Ones place
        }
        Debug.Log($"Left Operand ({operationGenerator.LeftOperand}): {string.Join(", ", expectedNumbers)}");

        // Right Operand
        if (operationGenerator.RightOperand >= 10)
        {
            expectedNumbers.Add(operationGenerator.RightOperand / 10);  // Tens place
            expectedNumbers.Add(operationGenerator.RightOperand % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Leave tens place empty for single-digit
            expectedNumbers.Add(operationGenerator.RightOperand);  // Ones place
        }
        Debug.Log($"Right Operand ({operationGenerator.RightOperand}): {string.Join(", ", expectedNumbers)}");

        // Expected Result Calculation
        int expectedResult = 0;
        switch (operationGenerator.operationType)
        {
            case OperationType.Addition:
                expectedResult = operationGenerator.LeftOperand + operationGenerator.RightOperand;
                break;
            case OperationType.Subtraction:
                expectedResult = operationGenerator.LeftOperand - operationGenerator.RightOperand;
                break;
            case OperationType.Multiplication:
                expectedResult = operationGenerator.LeftOperand * operationGenerator.RightOperand;
                break;
        }

        Debug.Log($"Computed Result ({operationGenerator.LeftOperand} {operationGenerator.operationType} {operationGenerator.RightOperand}): {expectedResult}");

        // Result tens and ones places
        if (expectedResult >= 10)
        {
            expectedNumbers.Add(expectedResult / 10);  // Tens place
            expectedNumbers.Add(expectedResult % 10);  // Ones place
        }
        else
        {
            expectedNumbers.Add(null);  // Leave tens place empty for single-digit result
            expectedNumbers.Add(expectedResult);  // Ones place
        }

        // Final expected numbers log for confirmation
        Debug.Log("Expected Numbers (Final): " + string.Join(", ", expectedNumbers));

        return expectedNumbers;
    }



}
