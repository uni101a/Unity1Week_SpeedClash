using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesPool : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> notesList = new Dictionary<string, List<GameObject>>()
    {
        {"Red", new List<GameObject>()},
        {"Green", new List<GameObject>()},
        {"Blue", new List<GameObject>()},
    };
    private Dictionary<string, GameObject> resourcesNotes = new Dictionary<string, GameObject>();

    [SerializeField] int initializeProductNum = 7;

    private void Start()
    {
        resourcesNotes.Add("Red", (GameObject)Resources.Load("RedNote"));
        resourcesNotes.Add("Green", (GameObject)Resources.Load("GreenNote"));
        resourcesNotes.Add("Blue", (GameObject)Resources.Load("BlueNote"));
    }

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

    public void ReleaseObject(GameObject note)
    {
        note.SetActive(false);
    }

    public void InitializedPool()
    {
        foreach(string key in resourcesNotes.Keys)
        {
            for(int n=0; n<initializeProductNum; n++)
            {
                GameObject note = (GameObject)Instantiate(resourcesNotes[key]);
                note.transform.parent = transform;
                notesList[key].Add(note);
            }
        }
    }
}
