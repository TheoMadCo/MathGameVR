using System.Collections.Generic;
using UnityEngine;

public class ValidationSystem_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public SlotManager_CompleteOperation slotManager;

    // This function checks the result and highlights incorrect cards
    public bool ValidateResult()
    {
        List<int?> placedNumbers = slotManager.GetPlacedCardNumbers();
        List<int?> expectedNumbers = GetExpectedNumbers();

        bool isAllCorrect = true;  // Track if all numbers are correct

        // Compare each slot, marking incorrect ones
        for (int i = 0; i < expectedNumbers.Count; i++)
        {
            int? expectedDigit = expectedNumbers[i];
            int? placedDigit = placedNumbers[i];

            if (expectedDigit != placedDigit)
            {
                isAllCorrect = false;  // At least one digit is incorrect
                slotManager.HighlightIncorrectSlot(i);  // Highlight the incorrect slot
            }
            else
            {
                slotManager.ClearHighlightSlot(i);  // Clear highlight for correct slot
            }
        }

        return isAllCorrect;  // Returns true if all numbers are correct
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
