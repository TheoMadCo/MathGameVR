using UnityEngine;
using TMPro;

public class OperationGenerator : MonoBehaviour
{
    public OperationType operationType;  // The selected operation type
    public TextMeshPro operatorText; // The UI element to display the operator

    public int LeftOperand { get; private set; }
    public int RightOperand { get; private set; }
    public int Result { get; private set; }  // Stores the result based on the operation

    public string OperationString => $"{LeftOperand} {GetOperatorSymbol()} {RightOperand} =";

    void Start()
    {
        
    }

    // Generate the operands and result based on the selected operation
    public void GenerateNewOperation()
    {
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
        LeftOperand = Random.Range(0, 50);
        RightOperand = Random.Range(0, 50);
        Result = LeftOperand + RightOperand;
    }

    // Generates a subtraction operation with non-negative results
    private void GenerateSubtraction()
    {
        LeftOperand = Random.Range(0, 50);
        RightOperand = Random.Range(0, LeftOperand + 1);  // Right operand cannot be greater than left operand
        Result = LeftOperand - RightOperand;
    }

    // Generates a multiplication operation (1-digit operands)
    private void GenerateMultiplication()
    {
        LeftOperand = Random.Range(1, 10);  // 1-9
        RightOperand = Random.Range(1, 10); // 1-9
        Result = LeftOperand * RightOperand;
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
