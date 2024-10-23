using UnityEngine;

public class GameManager_CompleteOperation : MonoBehaviour
{
    public OperationGenerator operationGenerator;
    public UIManager_CompleteOperation uiManager;
    public ValidationSystem_CompleteOperation validationSystem;
    public SlotManager_CompleteOperation slotManager;

    // Start is called before the first frame update
    void Start()
    {
        // Generate the new operation
        operationGenerator.GenerateNewOperation();

        // Display the operation on the UI
        uiManager.DisplayOperation(operationGenerator.OperationString);
    }

    // Called when the "Check Result" button is pressed
    public void OnCheckResultButtonPressed()
    {
        // Validate the result regardless of whether all slots are filled
        bool isCorrect = validationSystem.ValidateResult();
        uiManager.DisplayResult(isCorrect);  // Show "You Win" or "You Lose" on the UI
    }
}
