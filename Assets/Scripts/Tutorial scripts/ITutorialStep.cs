public interface ITutorialStep
{
    void StartStep(TutorialManager manager); // Called when the step starts
    void EndStep(); // Called when the step ends or needs to hide its canvas
}
