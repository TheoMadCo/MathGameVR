using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public GameObject background;      // The background GameObject
        public Button continueButton;      // The continue button for this step
    }

    [Header("Tutorial Steps")]
    [SerializeField] private TutorialStep[] tutorialSteps;  // Array of tutorial steps

    private int currentStep = 0;
    public Canvas tutorialCanvas;

    private void Awake()
    {

        // Set up continue button listeners
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            int stepIndex = i; // Capture the index for the lambda
            tutorialSteps[i].continueButton.onClick.AddListener(() => OnContinueClicked(stepIndex));
        }

        // Initialize tutorial state
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        // Disable all backgrounds except the first one
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].background.SetActive(i == 0);
        }

        currentStep = 0;
    }

    private void OnContinueClicked(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Length - 1)
        {
            // If this is the last step, disable the canvas
            tutorialCanvas.enabled = false;
            return;
        }

        // Disable current background
        tutorialSteps[stepIndex].background.SetActive(false);

        // Move to next step
        currentStep = stepIndex + 1;

        // Enable next background
        tutorialSteps[currentStep].background.SetActive(true);
    }

}