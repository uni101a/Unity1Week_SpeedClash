using System;

/// <summary>
/// シングルトンクラス
/// ゲームの全シーンを通してスコアを保持する
/// </summary>
public class Score
{
    public static Score _score = null;

    public enum ACCURACY
    {
        PERFECT = 5,
        GREAT = 3,
        BAD = 1,
    }

    //トータルスコア
    private int totalScore = 0;
    public int TotalScore
    {
        get { return totalScore; }
        set { totalScore = value; }
    }

    /// <summary>
    /// コンボ数
    /// クラッシュ精度がPerfect, Greatのとき+１ Badなら0にする
    /// </summary>
    private int combo = 0;
    public int Combo
    {
        get { return combo; }
        set { combo = value; }
    }

    /// <summary>
    /// コンボ数に応じてスコアに倍率をかける
    /// </summary>
    private int comboBonus = 1;
    public int ComboBonus
    {
        get { return comboBonus; }
        set { comboBonus = value; }
    }

    //ノーツ単体のスコア
    private int NOTE_SCORE = 100;

    private Score()
    {

    }

    public static Score GetInstance()
    {
        if(_score == null)
        {
            _score = new Score();
        }

        return _score;
    }

    /// <summary>
    /// トータルスコアを更新
    /// </summary>
    /// <param name="acc">精度</param>
    public int AddScore(int acc, int feaver)
    {
        if (combo == 15)
            ComboBonus *= 2;
        if (combo == 40)
            ComboBonus *= 2;

        int score = NOTE_SCORE * acc * ComboBonus * feaver;
        TotalScore += score;
        return score;
    }

    public void Initialize()
    {
        _score = null;
    }

    public void ResetCombo()
    {
        Combo = 0;
        ComboBonus = 1;
    }
}