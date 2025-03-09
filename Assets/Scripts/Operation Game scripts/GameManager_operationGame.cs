using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_operationGame : MonoBehaviour
{
    [Header("Game Settings")]
    public int maxRounds = 10;

    [Header("Point System")]
    private int currentRoundPoints = 0;
    private int totalPoints = 0;
    private int currentRoundErrors = 0;

    [Header("Difficulty Settings")]
    private int minRange = 1;
    private int maxRange = 11;

    private int correctAnswer;
    private int roundCount;
    private bool isQuestionAnsweredCorrectly = false;

    private string selectedDifficulty;
    private string selectedOperation;
    private string userName;

    [Header("Audio elements")]
    public AudioSource audioSource;     // Audio source component
    public AudioClip winSound;          // Sound played when winning
    public AudioClip loseSound;         // Sound played when losing
    public AudioClip newRoundSound;     // Sound played when winning
    public AudioClip endGameSound;      // Sound played when losing

    // Reference to UIManager
    private UIManager_operationGame uiManager;

    // Reference to LeaderBoard
    [SerializeField] private LeaderBoard_operationGame leaderBoard;

    void Start()
    {
        // Get the UIManager component
        uiManager = GetComponent<UIManager_operationGame>();

        // Fetch the selected difficulty and operation from PlayerPrefs
        selectedDifficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        selectedOperation = PlayerPrefs.GetString("Operation", "Addition");
        userName = PlayerPrefs.GetString("UserName", "Player");

        SetDifficultyRanges();
        uiManager.DisplayDifficulty(selectedDifficulty);

        roundCount = 0;
        totalPoints = 0;
        uiManager.ShowResultPanel(false, maxRounds);
        uiManager.SetContinueButtonActive(false);

        // Verify LeaderBoard reference
        if (leaderBoard == null)
        {
            Debug.LogError("LeaderBoard_operationGame reference not set in inspector!");
        }

        // Set the number of turns based on the selected operation and difficulty
        if (selectedOperation == "Multiplication")
        {
            switch (selectedDifficulty)
            {
                case "Easy":
                    maxRounds = 3;
                    break;
                case "Medium":
                    maxRounds = 5;
                    break;
                case "Hard":
                    maxRounds = 8;
                    break;
            }
        }

        GenerateQuestion();
    }

    void SetDifficultyRanges()
    {
        switch (selectedDifficulty)
        {
            case "Easy":
                minRange = 1;
                maxRange = 10;
                break;
            case "Medium":
                minRange = 1;
                maxRange = 20;
                break;
            case "Hard":
                minRange = 1;
                maxRange = 30;
                break;
        }
    }

    private List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    private List<int> GenerateAdditionAnswers()
    {
        int a, b;
        List<int> answers = new List<int>();

        switch (selectedDifficulty)
        {
            case "Easy":
                // Easy: Simple sums with fully random wrong answers
                a = Random.Range(minRange, maxRange);
                b = Random.Range(minRange, maxRange);
                correctAnswer = a + b;
                uiManager.DisplayQuestion($"{a} + {b} = ?");

                answers.Add(correctAnswer);
                while (answers.Count < 3)
                {
                    int wrongAnswer = Random.Range(minRange, maxRange * 2);
                    if (!answers.Contains(wrongAnswer))
                    {
                        answers.Add(wrongAnswer);
                    }
                }
                break;

            case "Medium":
            case "Hard":
                // Medium/Hard: One wrong answer with same units digit, one completely random
                a = Random.Range(minRange, maxRange + (selectedDifficulty == "Hard" ? 20 : 0));
                b = Random.Range(minRange, maxRange + (selectedDifficulty == "Hard" ? 20 : 0));
                correctAnswer = a + b;
                uiManager.DisplayQuestion($"{a} + {b} = ?");

                answers.Add(correctAnswer);

                // Generate a wrong answer with the same units digit
                int semiRandomWrongAnswer;
                do
                {
                    int tensDigit = Random.Range(0, 10);
                    semiRandomWrongAnswer = tensDigit * 10 + (correctAnswer % 10);
                } while (semiRandomWrongAnswer == correctAnswer);
                answers.Add(semiRandomWrongAnswer);

                // Generate a completely random wrong answer
                int completelyRandomWrongAnswer;
                do
                {
                    completelyRandomWrongAnswer = Random.Range(minRange, maxRange * 2);
                } while (answers.Contains(completelyRandomWrongAnswer));
                answers.Add(completelyRandomWrongAnswer);
                break;
        }

        // Shuffle the answers
        return ShuffleList(answers);
    }

    private List<int> GenerateSubtractionAnswers()
    {
        int a, b;
        List<int> answers = new List<int>();

        switch (selectedDifficulty)
        {
            case "Easy":
                // Easy: Simple subtraction with fully random wrong answers
                a = Random.Range(minRange, maxRange);
                b = Random.Range(minRange, a + 1); // Ensure b <= a
                correctAnswer = a - b;
                uiManager.DisplayQuestion($"{a} - {b} = ?");

                answers.Add(correctAnswer);
                while (answers.Count < 3)
                {
                    int wrongAnswer = Random.Range(minRange, maxRange);
                    if (!answers.Contains(wrongAnswer))
                    {
                        answers.Add(wrongAnswer);
                    }
                }
                break;

            case "Medium":
            case "Hard":
                // Medium/Hard: One wrong answer with same units digit, one completely random
                int maxUpperBound = selectedDifficulty == "Hard" ? 50 : 30;
                a = Random.Range(10, maxUpperBound + 1); // Start from 10 to ensure numbers above 10 (strange bug, idk)
                b = Random.Range(minRange, a + 1); // Ensure b <= a
                correctAnswer = a - b;
                uiManager.DisplayQuestion($"{a} - {b} = ?");

                answers.Add(correctAnswer);

                // Generate a wrong answer with the same units digit
                int semiRandomWrongAnswer;
                do
                {
                    int tensDigit = Random.Range(0, 10);
                    semiRandomWrongAnswer = tensDigit * 10 + (correctAnswer % 10);
                } while (semiRandomWrongAnswer == correctAnswer);
                answers.Add(semiRandomWrongAnswer);

                // Generate a completely random wrong answer
                int completelyRandomWrongAnswer;
                do
                {
                    completelyRandomWrongAnswer = Random.Range(minRange, maxUpperBound * 2);
                } while (answers.Contains(completelyRandomWrongAnswer));
                answers.Add(completelyRandomWrongAnswer);
                break;
        }

        // Shuffle the answers
        return ShuffleList(answers);
    }

    private List<int> GenerateMultiplicationAnswers()
    {
        int a, b;
        List<int> answers = new List<int>();

        switch (selectedDifficulty)
        {
            case "Easy":
                // Easy: Multiplication with tables up to 10, fully random wrong answers
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                correctAnswer = a * b;
                uiManager.DisplayQuestion($"{a} x {b} = ?");

                answers.Add(correctAnswer);
                while (answers.Count < 3)
                {
                    int wrongAnswer = Random.Range(1, 121); // Max possible product of 10 * 11
                    if (!answers.Contains(wrongAnswer))
                    {
                        answers.Add(wrongAnswer);
                    }
                }
                break;

            case "Medium":
            case "Hard":
                // Medium/Hard: Multiplication with tables up to 10, one wrong answer with same units digit
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                correctAnswer = a * b;
                uiManager.DisplayQuestion($"{a} x {b} = ?");

                answers.Add(correctAnswer);

                // Generate a wrong answer with the same units digit
                int semiRandomWrongAnswer;
                do
                {
                    int tensDigit = Random.Range(0, 10);
                    semiRandomWrongAnswer = tensDigit * 10 + (correctAnswer % 10);
                } while (semiRandomWrongAnswer == correctAnswer);
                answers.Add(semiRandomWrongAnswer);

                // Generate a completely random wrong answer
                int completelyRandomWrongAnswer;
                do
                {
                    completelyRandomWrongAnswer = Random.Range(1, 121);
                } while (answers.Contains(completelyRandomWrongAnswer));
                answers.Add(completelyRandomWrongAnswer);
                break;
        }

        // Shuffle the answers
        return ShuffleList(answers);
    }

    public void GenerateQuestion()
    {
        if (roundCount >= maxRounds)
        {
            ShowResult();
            return;
        }

        // Reset question-specific variables
        isQuestionAnsweredCorrectly = false;
        currentRoundErrors = 0;
        currentRoundPoints = 0;
        uiManager.SetContinueButtonActive(false);
        uiManager.ShowFeedbackMessage("");
        uiManager.ResetButtonAppearance();

        List<int> answers = new List<int>();

        switch (selectedOperation)
        {
            case "Addition":
                answers = GenerateAdditionAnswers();
                break;
            case "Subtraction":
                answers = GenerateSubtractionAnswers();
                break;
            case "Multiplication":
                answers = GenerateMultiplicationAnswers();
                break;
        }

        // Setup answer buttons with the generated answers
        uiManager.SetupAnswerButtons(answers, OnAnswerSelected);
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        // If already answered correctly, do nothing
        if (isQuestionAnsweredCorrectly)
            return;

        if (selectedAnswer == correctAnswer)
        {
            HandleCorrectAnswer(selectedAnswer);
        }
        else
        {
            HandleIncorrectAnswer(selectedAnswer);
        }
    }

    void HandleCorrectAnswer(int selectedAnswer)
    {
        // Calculate points based on errors
        switch (currentRoundErrors)
        {
            case 0:
                currentRoundPoints = 3;
                break;
            case 1:
                currentRoundPoints = 2;
                break;
            case 2:
                currentRoundPoints = 1;
                break;
            default:
                currentRoundPoints = 0;
                break;
        }

        totalPoints += currentRoundPoints;

        // Mark the correct answer button
        uiManager.MarkAnswerButton(selectedAnswer, true);

        isQuestionAnsweredCorrectly = true;
        uiManager.SetContinueButtonActive(true);
        uiManager.ShowFeedbackMessage(GetPositiveFeedbackMessage() + $"\nHai guadagnato: <color=#0fd1cb>{currentRoundPoints}</color> punti su 3");
        audioSource.PlayOneShot(winSound, 1);
    }

    void HandleIncorrectAnswer(int selectedAnswer)
    {
        currentRoundErrors++;
        // Mark the incorrect answer button
        uiManager.MarkAnswerButton(selectedAnswer, false);
        uiManager.ShowFeedbackMessage(GetRetryMessage());
        audioSource.PlayOneShot(loseSound, 1);
    }

    public void OnContinueButtonPressed()
    {
        if (isQuestionAnsweredCorrectly)
        {
            roundCount++;
            GenerateQuestion();
            audioSource.PlayOneShot(newRoundSound, 1);
        }
    }

    string GetPositiveFeedbackMessage()
    {
        string[] messages = {
            "Ottimo lavoro! Risposta corretta!",
            "Fantastico! La risposta è corretta!",
            "Risposta giusta! Continua così",
            "Molto bene! Risposta giusta"
        };
        return messages[Random.Range(0, messages.Length)];
    }

    string GetRetryMessage()
    {
        string[] messages = {
            "Ci sei quasi! Riprova",
            "Ci sei vicino. Prova di nuovo!",
            "Oops! Prova un'altra volta.",
            "Non mollare! Ci sei quasi!"
        };
        return messages[Random.Range(0, messages.Length)];
    }

    void ShowResult()
    {
        // Save to leaderboard before showing results
        if (leaderBoard != null)
        {
            leaderBoard.AddEntry(
                userName,
                selectedDifficulty,
                selectedOperation,
                totalPoints,
                maxRounds * 3 // maximum possible points (3 points per round)
            );

            // Export to CSV after adding the entry
            leaderBoard.ExportToCSV();

            Debug.Log($"Game completed - Score saved: {totalPoints} points for user {userName}");
        }
        else
        {
            Debug.LogError("LeaderBoard reference is null - score not saved!");
        }

        uiManager.ShowResultPanel(true, maxRounds, totalPoints, maxRounds * 3);
        audioSource.PlayOneShot(endGameSound, 1);
    }
}