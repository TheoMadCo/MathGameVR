using UnityEngine;
using TMPro;

public class OperationGenerator : MonoBehaviour
{
    public OperationType operationType;  // The selected operation type
    public TextMeshPro operatorText; // The UI element to display the operator

    public int LeftOperand { get; private set; }
    public int RightOperand { get; private set; }
    public int Result { get; private set; }  // Stores the result based on the operation

    private Difficulty currentDifficulty;  // Set by GameManager

    public string OperationString => $"{LeftOperand} {GetOperatorSymbol()} {RightOperand}";


    void Start()
    {
        
    }

    // Set the difficulty level (called by GameManager)
    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
    }

    public void SetOperationType(OperationType operationType)
    {
        this.operationType = operationType;
        Debug.Log($"Operation type set to: {operationType}");
    }


    public Difficulty CurrentDifficulty => currentDifficulty; // Getter for current difficulty that can be accessed by other scripts keeping encapsulation


    // Generate the operands and result based on the selected operation
    public void GenerateNewOperation()
    {
        Debug.Log($"Generating new operation. Type: {operationType}, Difficulty: {currentDifficulty}");

        switch (operationType)
        {
            case OperationType.Addition:
                GenerateAddition();
                operatorText.text = "+";  // Update operator symbol
                break;

            case OperationType.Subtraction:
                GenerateSubtraction();
                operatorText.text = "-";
                break;

            case OperationType.Multiplication:
                GenerateMultiplication();
                operatorText.text = "×";
                break;
        }

        // Debugging logs to verify the generated values
        Debug.Log($"Generated Operation: {LeftOperand} {GetOperatorSymbol()} {RightOperand} = {Result}");
    }

    // Generates an addition operation
    private void GenerateAddition()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                // Avoid carryover and limit result to ≤ 100
                do
                {
                    LeftOperand = Random.Range(10, 50);
                    RightOperand = Random.Range(10, 50);
                    Result = LeftOperand + RightOperand;
                } while (Result > 100 || HasCarryover(LeftOperand, RightOperand));
                break;

            case Difficulty.Medium:
                // Allow carryover and limit result to two digits
                do
                {
                    LeftOperand = Random.Range(10, 99);
                    RightOperand = Random.Range(10, 99);
                    Result = LeftOperand + RightOperand;
                } while (Result > 99);
                break;

            case Difficulty.Hard:
                // Allow carryover and limit result to three digits
                do
                {
                    LeftOperand = Random.Range(100, 999);
                    RightOperand = Random.Range(100, 999);
                    Result = LeftOperand + RightOperand;
                } while (Result > 999);  // Ensure result does not exceed 999
                break;
        }
    }


    private void GenerateSubtraction()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                // No borrowing, 2-digit numbers
                do
                {
                    LeftOperand = Random.Range(10, 99);
                    RightOperand = Random.Range(10, LeftOperand);  // Ensure LeftOperand > RightOperand
                } while (HasBorrowing(LeftOperand, RightOperand));
                Result = LeftOperand - RightOperand;
                break;

            case Difficulty.Medium:
                // Allow borrowing, 2-digit numbers
                LeftOperand = Random.Range(10, 99);
                RightOperand = Random.Range(10, LeftOperand);
                Result = LeftOperand - RightOperand;
                break;

            case Difficulty.Hard:
                // Allow borrowing, 3-digit numbers
                LeftOperand = Random.Range(100, 999);
                RightOperand = Random.Range(100, LeftOperand);
                Result = LeftOperand - RightOperand;
                break;
        }
    }

    private void GenerateMultiplication()
    {
        // Multiplication remains unchanged for all difficulties
        LeftOperand = Random.Range(2, 9);  // Single-digit numbers
        RightOperand = Random.Range(2, 9);
        Result = LeftOperand * RightOperand;
    }

    // Helper function to detect carryover in addition
    private bool HasCarryover(int left, int right)
    {
        return (left % 10 + right % 10) >= 10;
    }

    // Helper function to detect borrowing in subtraction
    private bool HasBorrowing(int left, int right)
    {
        return (left % 10) < (right % 10);
    }

// Get the operator symbol for the current operation
private string GetOperatorSymbol()
    {
        switch (operationType)
        {
            case OperationType.Addition: return "+";
            case OperationType.Subtraction: return "-";
            case OperationType.Multiplication: return "×";
            default: return "?";
        }
    }
}
