using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private NotesManager _noteManager;
    //このオブジェクトがステージ中何番目のノーツかを保持する
    private int noteNumber = 0;
    //1区切りに対してのノーツの座標
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    /// <summary>
    /// ノーツオブジェクトの現在の状態
    /// </summary>
    private enum NOTE_STATE
    {
        IDLING, //待機中
        MOVING, //移動中
    }
    private NOTE_STATE _noteState = NOTE_STATE.IDLING;

    // Start is called before the first frame update
    void Start()
    {
        _noteManager = transform.parent.transform.parent.GetComponent<NotesManager>();
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

        Move(transform.position, endPos, Time.deltaTime * _noteManager.NoteSpeed);
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
    public void DestroyProparties()
    {
        noteNumber = -1;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        //ノーツがデストロイゾーンに入ったときオブジェクトを非アクティブ化
        if(other.gameObject.tag == "DestroyZone")
        {
            DestroyProparties();
            _noteManager.ReleaseNote(gameObject);
        }
    }
}
