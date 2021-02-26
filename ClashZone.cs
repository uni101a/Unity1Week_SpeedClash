using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClashZone : MonoBehaviour
{
    private Score _score;

    private float perfectZone = 0.01f;
    private float greatZone = 0.075f;

    private void Start()
    {
        _score = Score.GetInstance();
    }

    /// <summary>
    /// クラッシュ時の精度を返す    
    /// 5 → Perfect
    /// 3 → Great
    /// 1 → Bad
    /// </summary>
    /// <returns>精度</returns>
    public int GetAcc(GameObject clashedNote)
    {
        float distance = Math.Abs(transform.position.x - clashedNote.transform.position.x);

        int acc = -1;

        if(distance < perfectZone)
        {
            acc = (int)Score.ACCURACY.PERFECT;
        }
        else if (distance < greatZone)
        {
            acc = (int)Score.ACCURACY.GREAT;
        }
        else
        {
            acc = (int)Score.ACCURACY.BAD;
        }

        return acc;
    }
}
