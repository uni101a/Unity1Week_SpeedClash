using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _comboText;

    private Score _score;
    private int totalScore = 0;
    private int combo = 0;
    private int MAX_DIGIT = 7;

    private void Start()
    {
        _score = Score.GetInstance();
        _comboText.text = "";
    }

    private void Update()
    {
        UpdateScore();
        UpdateCombo();
    }

    public void UpdateScore()
    {
        int scoreVal = _score.TotalScore;
        if (scoreVal == totalScore)
            return;

        totalScore = scoreVal;
        _scoreText.text = totalScore.ToString().PadLeft(MAX_DIGIT, '0');
    }

    public void UpdateCombo()
    {
        int comboVal = _score.Combo;
        if (comboVal == combo)
            return;

        combo = comboVal;
        if (combo == 0)
            _comboText.text = "";
        else
            _comboText.text = combo + " Combo!";
    }
}
