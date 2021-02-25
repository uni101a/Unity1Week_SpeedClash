using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotesManager : MonoBehaviour
{
    /**
    private List<Vector3> notesPosition = new List<Vector3>()
    {
        {new Vector3(1.212f, 1.455f, -0.34f)},
        {new Vector3(1.958f, 0.6f, -0.34f)},
        {new Vector3(1.958f, -0.6f, -0.34f)},
        {new Vector3(1.212f, -1.455f, -0.34f)},
        {new Vector3(0, -1.945f, -0.34f)},
        {new Vector3(-1.212f, -1.455f, -0.34f)},
    };*/
    private List<Vector3> notesPosition = new List<Vector3>()
    {
        {new Vector3(1.095f, 1.517f, -0.31f)},
        {new Vector3(1.717f, 0.6f, -0.31f)},
        {new Vector3(1.717f, -0.6f, -0.31f)},
        {new Vector3(1.095f, -1.6f, -0.31f)}, //y = -1.517f
        {new Vector3(0, -1.884f, -0.31f)}, //y = -1.93f
        {new Vector3(-1.095f, -1.517f, -0.31f)},
    };
    private NotesPool _notesPool;

    private bool isMoving = true;
    public bool GetIsMoving()
    {
        return isMoving;
    }

    private float noteSpeed = 2f;
    public float NoteSpeed
    {
        get { return noteSpeed; }
        set { noteSpeed = value; }
    }

    private float MAX_NOTESPEED = 7f;
    public float GET_MAX_NOTESPEED()
    {
        return MAX_NOTESPEED;
    }

    private int correctedNotesNum = 0;
    public int CorrectedNotesNum
    {
        get { return correctedNotesNum; }
        set { correctedNotesNum = value; }
    }

    private float _time = 0;

    private GameObject enableClashNote = null;
    public GameObject EnableClashNote
    {
        get { return enableClashNote; }
        set { enableClashNote = value; }
    }

    /// <summary>
    /// index ...
    /// 0 → プロダクティングノーツ
    ///  ... 
    /// 6 → デストロイノーツ
    /// </summary>
    /// <param name="index"></param>
    /// <returns>ノーツの座標</returns>
    public Vector3 GetNotesPosition(int index)
    {
        return notesPosition[index];
    }

    // Start is called before the first frame update
    void Start()
    {
        _notesPool = transform.GetChild(0).GetComponent<NotesPool>();
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }


    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime * noteSpeed;
        if(_time >= 1 - (0.05f + noteSpeed*0.01f) && isMoving)
        {
            isMoving = false;
            ProductNote();
            StartCoroutine(RestartMove(10, () =>
            {
                _time = 0;
                isMoving = true;
            }));
        }
    }

    private IEnumerator RestartMove(int delayFrameCount, Action action)
    {
        for(var i=0; i<delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }

    public void AcceralateNotes()
    {
        if (CorrectedNotesNum % 10 == 0)
            NoteSpeed = System.Math.Min(NoteSpeed + 0.2f, GET_MAX_NOTESPEED());
        Debug.Log(NoteSpeed);
    }

    public void ReleaseNote(GameObject releaseObject)
    {
        _notesPool.ReleaseObject(releaseObject);
    }

    private string DefineColor()
    {
        string colorKey = null;
        float rand = UnityEngine.Random.Range(0, 1.0f);

        if (0.0f <= rand && rand < 0.34f)
            colorKey = "Red";
        else if (0.34f <= rand && rand < 0.67f)
            colorKey = "Green";
        else
            colorKey = "Blue";

        return colorKey;
    }

    private void ProductNote()
    {
        GameObject newNote = _notesPool.GetObject(DefineColor());
        newNote.transform.position = notesPosition[0];
        newNote.SetActive(true);
        Note note = newNote.GetComponent<Note>();
        note.SetPosition(notesPosition[0], notesPosition[1]);
    }
}
