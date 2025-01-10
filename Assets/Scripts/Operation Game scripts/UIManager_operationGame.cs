using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Security;

public class UIManager_operationGame : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Button Colors")]
    [SerializeField] private Color correctAnswerColor = Color.green;
    [SerializeField] private Color incorrectAnswerColor = Color.red;
    [SerializeField] private Color defaultButtonColor = Color.white;
    [SerializeField] private Color defaultTextColor = Color.black;
    [SerializeField] private Color selectedTextColor = Color.white;

    // Reference to the GameManager to call methods
    private GameManager_operationGame gameManager;

    private void Awake()
    {
        // Get the GameManager component
        gameManager = GetComponent<GameManager_operationGame>();

        // Set up continue button listener
        continueButton.onClick.AddListener(OnContinueButtonPressed);
    }

    public void DisplayQuestion(string questionStr)
    {
        questionText.text = questionStr;
    }

    public void SetupAnswerButtons(List<int> answers, System.Action<int> onAnswerSelectedCallback)
    {
        // Shuffle the answers
        answers = ShuffleList(answers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer.ToString();

            // Remove any existing listeners first
            answerButtons[i].onClick.RemoveAllListeners();

            // Add new listener
            int capturedAnswer = answer;
            answerButtons[i].onClick.AddListener(() => onAnswerSelectedCallback(capturedAnswer));
        }
    }

    public void DisplayDifficulty(string difficulty)
    {
        // italianize the difficulty
        string currentDifficulty = difficulty == "Easy" ? "Facile" : difficulty == "Medium" ? "Media" : "Difficile"; // Italianized difficulty levels
        difficultyText.text = "Difficoltà: " + currentDifficulty;
    }

    public void ShowFeedbackMessage(string message)
    {
        feedbackText.text = message;
    }

    public void SetContinueButtonActive(bool isActive)
    {
        continueButton.gameObject.SetActive(isActive);
    }

    public void ResetButtonAppearance()
    {
        foreach (Button button in answerButtons)
        {
            button.image.color = defaultButtonColor;
            button.GetComponentInChildren<TextMeshProUGUI>().color = defaultTextColor;
        }
    }

    public void MarkAnswerButton(int answer, bool isCorrect)
    {
        foreach (Button button in answerButtons)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>().text == answer.ToString())
            {
                button.image.color = isCorrect ? correctAnswerColor : incorrectAnswerColor;
                button.GetComponentInChildren<TextMeshProUGUI>().color = selectedTextColor;
                break;
            }
        }
    }

    public void ShowGamePanel(bool show)
    {
        gamePanel.SetActive(show);
    }

    public void ShowResultPanel(bool show, int maxRounds)
    {
        gamePanel.SetActive(!show);
        resultPanel.SetActive(show);

        if (show)
        {
            resultText.text = $"Hai completato {maxRounds} rounds!\nBella esercitazione!";
        }
    }

    private void OnContinueButtonPressed()
    {
        gameManager.OnContinueButtonPressed();
    }

    // Helper method to shuffle a list (same as in GameManager)
    private List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}