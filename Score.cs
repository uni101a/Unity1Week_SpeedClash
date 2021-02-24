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
    public int ConboBonus
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
    public void AddScore(int acc)
    {
        if(acc == 1)
        {
            Combo = 0;
        }
        else
        {
            Combo++;
            if (combo == 5)
                ConboBonus *= 2;
            else if (combo == 10)
                ConboBonus *= 2;
            else if (combo == 20)
                ConboBonus *= 2;
        }

        TotalScore += NOTE_SCORE * (acc + ConboBonus);
    }

}