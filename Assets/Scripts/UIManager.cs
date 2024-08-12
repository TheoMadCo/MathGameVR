using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;

    public void UpdateQuestionText(string question)
    {
        questionText.text = question;
    }

    public void UpdateScoreText(float score)
    {
        scoreText.text = "Score: " + score;
    }
}
