using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] NotesManager _noteManager = default;
    [SerializeField] ClashZone _clashZone = default;
    [SerializeField] ColorStore _colorStore = default;

    [SerializeField] GameObject _mainColor;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _comboText;
    [SerializeField] Text _feaverText;
    [SerializeField] Text _addScoreText;
    [SerializeField] Slider _hpSlider;

    private Score _score;

    public static Color _color;
    public static bool IsGaming = false;
    private static bool isMissing = false;

    private static int hp = 0;
    private int MAX_HPVALUE = 4;
    private int MAX_DIGIT = 7;
    private int feaverBonus = 1;
    private int FEAVERVALUE = 2;


    // Start is called before the first frame update
    void Start()
    {
        _score = Score.GetInstance();
        _comboText.text = "";
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        SetMainColor();

        hp = MAX_HPVALUE;
        IsGaming = true;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム進行中かを判定
        if (!IsGaming)
            return;

        //Espキーを押してリログ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGaming = false;
            Reload();
        }

        //ゲーム終了処理
        if (hp <= 0)
        {
            LoadRanking();
            return;
        }

        // ------------------- uGUI更新 ---------------------

        //ミス判定
        if (isMissing)
        {
            MissedPross();
        }

        //フィーバーテキスト
        if (!_colorStore.IsFeaver && _feaverText.gameObject.activeInHierarchy == true)
            _feaverText.gameObject.SetActive(false);

        // ------------------- uGUI更新 ---------------------

        //ノーツクラッシュ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnInput();
        }
    }

    private void OnInput()
    {
        if (IsExist())
        {
            Clash();
        }
    }

    private void Clash()
    {
        //叩いたノーツの情報を取得
        GameObject clashedNote = _noteManager.EnableClashNote;
        clashedNote.GetComponent<Note>().ReleaseNote();
        Color clashColor = GetColor(clashedNote.name);

        //コンボ数を更新(++ or 0)し、色が違っていたらHPを減らして処理を終了
        UpdateCombo(clashColor);
        if (!IsCorrectColor(clashColor))
        {
            Missed();
            return;
        }

        //ノーツをストアに追加
        _noteManager.CorrectedNotesNum++;
        StoreColor(clashColor);

        //スコアを更新 フィーバー状態でフィーバー時の色とクラッシュノーツの色が一致していたら倍率をかける
        if (_colorStore.IsFeaver && clashColor == _colorStore.FeaverColor)
            feaverBonus = FEAVERVALUE;
        else
            feaverBonus = 1;
        UpdateScore(_clashZone.GetAcc(clashedNote));

        //正しくたたいたノーツ数が条件と一致しているとき、指示色を変更
        if (_noteManager.CorrectedNotesNum % 5 == 0)
            SetMainColor();
        //関数先の条件が一致するときノーツの速度を速める
        _noteManager.AcceralateNotes();
    }

    private bool IsExist()
    {
        if(_noteManager.EnableClashNote == null)
        {
            return false;
        }

        return true;
    }

    private bool IsCorrectColor(Color clashColor)
    {
        return clashColor == _color ? true : false;
    }

    private Color GetColor(string color)
    {
        switch (color)
        {
            case "RedNote(Clone)":
                return new Color(1, 0, 0);

            case "GreenNote(Clone)":
                return new Color(0, 1, 0);

            case "BlueNote(Clone)":
                return new Color(0, 0, 1);

            default:
                break;
        }
        return new Color(1, 1, 1);
    }

    private void StoreColor(Color clashColor)
    {
        if (_colorStore.IsFeaver)
            return;

        _colorStore.Store(clashColor);

        if (_colorStore.IsMaxStored())
        {
            _colorStore.OnFeaver();
            _feaverText.gameObject.SetActive(true);
            _feaverText.color = _colorStore.FeaverColor;
        }
    }

    private void UpdateScore(int accuracy)
    {
        int score = _score.AddScore(accuracy, feaverBonus);
        _addScoreText.text = "+" + score;
        _scoreText.text = _score.TotalScore.ToString().PadLeft(MAX_DIGIT, '0');
    }

    private void UpdateCombo(Color judgeColor)
    {
        if (judgeColor == _color)
        {
            _score.Combo++;
            _comboText.text = _score.Combo + " Combo!";
        }
        else
        {
            _score.ResetCombo();
            _comboText.text = "";
        }
    }

    private void MissedPross()
    {
        isMissing = false;
        _comboText.text = "";
        _addScoreText.text = "Miss!";
        _hpSlider.value -= 1.0f / MAX_HPVALUE;
    }

    private void SetMainColor()
    {
        int rand = UnityEngine.Random.Range(1, 4);
        if(rand == 1)
        {
            _color = new Color(1, 0, 0);
        }
        else if (rand == 2)
        {
            _color = new Color(0, 1, 0);
        }
        else if (rand == 3)
        {
            _color = new Color(0, 0, 1);
        }
        else
        {
            _color = new Color(1, 1, 1);
        }

        _mainColor.GetComponent<Renderer>().material.SetColor("_MainColor", _color);
    }

    private void LoadRanking()
    {
        IsGaming = false;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(_score.TotalScore);

        StartCoroutine("Restart");
    }

    private void Reload()
    {
        _score.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(10f);
        Reload();
    }

    public static bool IsMissed(Color missColor)
    {
        return missColor == _color ? true : false;
    }

    public static void Missed()
    {
        isMissing = true;
        hp--;
    }
}
