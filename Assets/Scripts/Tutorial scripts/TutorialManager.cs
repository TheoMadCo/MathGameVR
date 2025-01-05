using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Tooltip("Drag and drop all tutorial steps here in the order they should play.")]
    public List<MonoBehaviour> tutorialSteps; // Each step must implement ITutorialStep

    [Tooltip("Canvas to display when the tutorial is completed.")]
    public Canvas endTutorialCanvas;

    private int currentStepIndex = -1;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
    }

    private void Start()
    {
        // Hide the end tutorial canvas initially
        if (endTutorialCanvas != null)
        {
            endTutorialCanvas.enabled = false;
        }

        ProceedToNextStep();
    }

    public void ProceedToNextStep()
    {
        // Disable the previous step’s canvas
        if (currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count)
        {
            (tutorialSteps[currentStepIndex] as ITutorialStep)?.EndStep();
        }

        currentStepIndex++;

        if (currentStepIndex < tutorialSteps.Count)
        {
            // Disable all canvases
            DisableAllCanvases();

            // Enable the current step’s logic
            (tutorialSteps[currentStepIndex] as ITutorialStep)?.StartStep(this);
        }
        else
        {
            // Tutorial completed, show the end game canvas
            ShowEndTutorialCanvas();
        }
    }

    private void DisableAllCanvases()
    {
        foreach (var step in tutorialSteps)
        {
            if (step is ITutorialStep tutorialStep)
            {
                tutorialStep.EndStep(); // Ensure all steps clean up their canvases
            }
        }
    }

    private void ShowEndTutorialCanvas()
    {
        if (endTutorialCanvas != null)
        {
            endTutorialCanvas.enabled = true;
        }

        Debug.Log("Tutorial completed! Showing the end game canvas.");
    }
}
