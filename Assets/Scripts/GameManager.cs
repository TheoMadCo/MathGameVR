using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI DifficultyText;
    public Button[] answerButtons;
    public GameObject gamePanel;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public Button retryButton;
    public Button quitGameButton;

    private int correctAnswer;
    private float score;
    private int roundCount;
    private const int maxRounds = 10;

    private int minRange = 1;
    private int maxRange = 11;

    void Start()
    {
        SetDifficultyRanges();
        displayDifficulty();
        score = 0;
        roundCount = 0;
        resultPanel.SetActive(false);
        GenerateQuestion();
    }

    void SetDifficultyRanges()
    {
        switch (SelectionManager.selectedDifficulty)
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

    void GenerateQuestion()
    {
        if (roundCount >= maxRounds)
        {
            ShowResult();
            return;
        }

        int a, b;
        string operation = SelectionManager.selectedOperation;
        List<int> answers = new List<int>();

        switch (operation)
        {
            case "Addition":
                a = Random.Range(minRange, maxRange);
                b = Random.Range(minRange, maxRange);
                correctAnswer = a + b;
                questionText.text = $"{a} + {b} = ?";
                break;
            case "Subtraction":
                a = Random.Range(minRange, maxRange);
                b = Random.Range(minRange, a + 1); // Ensure b <= a
                correctAnswer = a - b;
                questionText.text = $"{a} - {b} = ?";
                break;
            case "Multiplication":
                a = Random.Range(minRange, maxRange);
                b = Random.Range(minRange, maxRange);
                correctAnswer = a * b;
                questionText.text = $"{a} * {b} = ?";
                break;
            case "Division":
                b = Random.Range(minRange, maxRange);
                correctAnswer = Random.Range(minRange, maxRange);
                a = b * correctAnswer; // Ensure a is divisible by b
                questionText.text = $"{a} / {b} = ?";
                break;
        }

        answers.Add(correctAnswer);

        while (answers.Count < 3)
        {
            int wrongAnswer = Random.Range(minRange, maxRange * 2); // Ensure the wrong answers are within a reasonable range
            if (!answers.Contains(wrongAnswer))
            {
                answers.Add(wrongAnswer);
            }
        }

        answers = ShuffleList(answers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer.ToString();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer));
        }

        roundCount++;
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            score += 1;
        }
        else
        {
            score -= 0.25f;
        }

        scoreText.text = "Score: " + score;
        ResetButtons();
        GenerateQuestion();
    }

    string GetScoreText()
    {
        if (score < 3)
        {
            return "Next time will go better!";
        }
        else if (score < 6)
        {
            return "Nice work!";
        }
        else if (score < 8)
        {
            return "Good job!";
        }
        else
        {
            return "Excellent!";
        }
    }

    void ResetButtons()
    {
        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    List<int> ShuffleList(List<int> list)
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

    void ShowResult()
    {
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
        string motivationalText = GetScoreText();
        resultText.text = $"{score} out of {maxRounds}\n" + motivationalText;
        retryButton.onClick.AddListener(RetryGame);
        quitGameButton.onClick.AddListener(BackToSelection);
    }

    void displayDifficulty()
     {
        DifficultyText.text += SelectionManager.selectedDifficulty;
    }
    void RetryGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void BackToSelection()
    {
        SceneManager.LoadScene("StartupScene");
    }
}
