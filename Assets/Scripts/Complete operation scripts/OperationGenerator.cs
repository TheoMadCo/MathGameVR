using UnityEngine;

public class OperationGenerator : MonoBehaviour
{
    public int LeftOperand { get; private set; }
    public int RightOperand { get; private set; }
    public int Result => LeftOperand + RightOperand;  // Addition operation result

    public string OperationString => $"{LeftOperand} + {RightOperand} =";

    // Generates a random addition operation
    public void GenerateNewOperation()
    {
        LeftOperand = Random.Range(0, 50);  // Generate random operand (0-50)
        RightOperand = Random.Range(0, 49);  // Generate random operand (0-50)
    }
}
