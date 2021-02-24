using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノーツオブジェクトプール
/// </summary>
public class NotesPool : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> notesList = new Dictionary<string, List<GameObject>>()
    {
        {"Red", new List<GameObject>()},
        {"Green", new List<GameObject>()},
        {"Blue", new List<GameObject>()},
    };
    private Dictionary<string, GameObject> resourcesNotes = new Dictionary<string, GameObject>();

    //初期化するオブジェクトの数
    [SerializeField] int initializeProductNum = 7;

    private void Start()
    {
        resourcesNotes.Add("Red", (GameObject)Resources.Load("RedNote"));
        resourcesNotes.Add("Green", (GameObject)Resources.Load("GreenNote"));
        resourcesNotes.Add("Blue", (GameObject)Resources.Load("BlueNote"));

        InitializedPool();
    }

    /// <summary>
    /// オブジェクトプールから非アクティブのオブジェクトを返す
    /// </summary>
    /// <param name="key">ノーツの色(Red, Green, Blue)</param>
    /// <returns>非アクティブのノーツ</returns>
    public GameObject GetObject(string key)
    {
        foreach (GameObject note in notesList[key])
        {
            if (note.activeInHierarchy == false)
            {
                note.SetActive(true);
                return note;
            }
        }

        GameObject newNote = (GameObject)Instantiate(resourcesNotes[key]);
        newNote.transform.parent = transform;
        notesList[key].Add(newNote);

        return newNote;
    }

    /// <summary>
    /// オブジェクトを非アクティブ化
    /// </summary>
    /// <param name="note">ゲームオブジェクト</param>
    public void ReleaseObject(GameObject note)
    {
        note.SetActive(false);
    }

    /// <summary>
    /// オブジェクトプールを初期化
    /// </summary>
    public void InitializedPool()
    {
        foreach(string key in resourcesNotes.Keys)
        {
            for(int n=0; n<initializeProductNum; n++)
            {
                GameObject note = (GameObject)Instantiate(resourcesNotes[key]);
                note.transform.parent = transform;
                notesList[key].Add(note);
                ReleaseObject(note);
            }
        }
    }
}
