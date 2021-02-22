using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductZone : MonoBehaviour
{
    [SerializeField] NotesPool _notesPool = default;

    public void Product()
    {
        string key = "Red";
        _notesPool.GetObject(key);
    }
}
