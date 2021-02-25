using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private NotesManager _noteManager;

    /// <summary>
    /// ノーツオブジェクトの現在の状態
    /// </summary>
    private enum NOTE_STATE
    {
        IDLING, //待機中
        MOVING, //移動中
    }
    private NOTE_STATE _noteState = NOTE_STATE.IDLING;

    private Score _score;

    private Color myColor;
    /// <summary>
    /// DestroyPropartiesで初期化する変数
    /// </summary>
    //このオブジェクトがステージ中何番目のノーツかを保持する
    private int noteNumber = 0;
    //1区切りに対してのノーツの座標
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _noteManager = transform.parent.transform.parent.GetComponent<NotesManager>();
        _score = Score.GetInstance();

        switch (gameObject.tag)
        {
            case "Red":
                myColor = new Color(1, 0, 0);
                break;
            case "Green":
                myColor = new Color(0, 1, 0);
                break;
            case "Blue":
                myColor = new Color(0, 0, 1);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        // ! SUPRE IF STREAM !
        if (!_noteManager.GetIsMoving())
        {
            if(_noteState == NOTE_STATE.MOVING)
            {
                _noteState = NOTE_STATE.IDLING;
                noteNumber++;
                SetPosition(transform.position, _noteManager.GetNotesPosition(noteNumber+1));
            }

            return;
        }

        if (_noteState == NOTE_STATE.IDLING)
            _noteState = NOTE_STATE.MOVING;

        Move(transform.position, endPos, Time.deltaTime * _noteManager.NoteSpeed * 1.5f);
    }

    /// <summary>
    /// 1区切りで移動するノーツの座標をセット
    /// </summary>
    /// <param name="start">スタート座標</param>
    /// <param name="end">移動後の座標</param>
    public void SetPosition(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;
    }

    /// <summary>
    /// ノーツを移動
    /// </summary>
    /// <param name="start">オブジェクトの現在座標</param>
    /// <param name="end">目的の座標</param>
    /// <param name="step">移動する値</param>
    public void Move(Vector3 start, Vector3 end, float step)
    {
        transform.position = Vector3.MoveTowards(start, end, step);
    }

    /// <summary>
    /// 変数を初期化
    /// </summary>
    private void DestroyProparties()
    {
        noteNumber = -1;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        _noteState = NOTE_STATE.MOVING; //IDLING状態でfalseにすると一回分動かなくなる

        if(_noteManager.EnableClashNote == gameObject)
        {
            _noteManager.EnableClashNote = null;
        }
    }

    /// <summary>
    /// ノーツオブジェクトと非アクティブにする
    /// </summary>
    public void ReleaseNote()
    {
        DestroyProparties();
        _noteManager.ReleaseNote(gameObject);
    }

    /// <summary>
    /// クラッシュゾーンに突入したときオブジェクトにクラッシュ判定を持たせる
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!Player.IsGaming)
            return;

        //ノーツがクラッシュゾーンと触れたか判定
        if(other.gameObject.tag == "ClashZone")
        {
            _noteManager.EnableClashNote = gameObject;
        }
    }

    /// <summary>
    /// ノーツオブジェクトがクラッシュゾーンから脱出したときクラッシュ判定を削除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!Player.IsGaming)
            return;

        //ノーツがクラッシュゾーンから離れたかを判定
        if (other.gameObject.tag == "ClashZone")
        {
            if (Player.IsMissed(myColor))
            {
                _score.ResetCombo();
                Player.Missed();
            }

            if(_noteManager.EnableClashNote == gameObject)
            {
                _noteManager.EnableClashNote = null;
            }
        }
    }

    /// <summary>
    /// ノーツオブジェクトがデストロイゾーンに突入したらオブジェクトをリリースする
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //ノーツがデストロイゾーンに入ったときオブジェクトを非アクティブ化
        if(other.gameObject.tag == "DestroyZone")
        {
            ReleaseNote();
        }
    }
}
