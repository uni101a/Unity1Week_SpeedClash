using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] NotesManager _noteManager = default;
    [SerializeField] ClashZone _clashZone = default;
    [SerializeField] ColorStore _colorStore = default;

    private Score _score;

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        _score = Score.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
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
            count++;
            if (count % 5 == 0)
            {
                float speed = _noteManager.NoteSpeed + 0.2f;
                _noteManager.NoteSpeed = System.Math.Min(speed, _noteManager.GET_MAX_NOTESPEED());
                //Debug.Log(_noteManager.NoteSpeed);
            }
        }
    }

    private bool IsExist()
    {
        if(_noteManager.EnableClashNote == null)
        {
            return false;
        }

        return true;
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

    private void Clash()
    {
        GameObject clashedNote = _noteManager.EnableClashNote;
        clashedNote.GetComponent<Note>().ReleaseNote();

        _colorStore.Store(GetColor(clashedNote.name));

        int acc = _clashZone.GetAcc(clashedNote);
        _score.AddScore(acc);
    }
}
