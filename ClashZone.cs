using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClashZone : MonoBehaviour
{
    private bool isOver = false;
    public bool IsOver
    {
        get{ return isOver; }
        set{ isOver = value; }
    }
    private GameObject overedNote = null;

    private float clashZoneX;
    private float perfectZone;
    private float greatZone;
    private float goodZone;
    private float badZone;

    private void Start()
    {
        clashZoneX = transform.position.x;
    }

    public GameObject GetOveredNote()
    {
        return overedNote;
    }

    /// <summary>
    /// return ...
    /// 0 → Perfect
    /// 1 → Great
    /// 2 → Good
    /// 3 → Bad
    /// </summary>
    /// <returns></returns>
    public int GetAcc()
    {
        ResetOverval();
        float noteLeftedge = overedNote.transform.position.x - (overedNote.transform.localScale.x / 2.0f);
        float distance = Math.Abs(clashZoneX - noteLeftedge);

        int acc = -1;

        if(distance < perfectZone)
        {
            acc = 0;
        }
        else if (distance < greatZone)
        {
            acc = 1;
        }
        else if (distance < goodZone)
        {
            acc = 2;
        }
        else if (distance < badZone)
        {
            acc = 3;
        }

        return acc;
    }

    private void ResetOverval()
    {
        isOver = false;
        overedNote = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        isOver = true;
        overedNote = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        ResetOverval();
    }
}
