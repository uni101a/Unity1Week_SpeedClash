using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorStore : MonoBehaviour
{
    private Material _material;
    private float storedLiquit = 0;
    private Color _color = new Color(0, 0, 0);
    public Color GetMixiedColor()
    {
        return _color;
    }

    private int maxMixingNum = 40;
    public int MaxMixingNum
    {
        get { return maxMixingNum; }
        set { maxMixingNum = value; }
    }

    private float FEAVERTIME = 5f;
    public float GET_FEAVERTIME()
    {
        return FEAVERTIME;
    }

    private bool isFeaver = false;
    public bool IsFeaver
    {
        get { return isFeaver; }
    }

    private Color feaverColor = new Color(1, 1, 1);
    public Color FeaverColor
    {
        get { return feaverColor; }
    }

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (isFeaver)
        {
            DecreaseStore();
        }
    }

    public void Store(Color clashColor)
    {
        UpLiquid();
        MixColor(clashColor);
    }

    public bool IsMaxStored()
    {
        return storedLiquit >= 1 ? true : false;
    }

    public bool IsEmpty()
    {
        return storedLiquit <= 0.0001 ? true : false;
    }

    private void UpLiquid()
    {
        storedLiquit += 1.0f / MaxMixingNum;
        if (storedLiquit > 1.0f)
            return;
        _material.SetFloat("_Height", storedLiquit);
    }

    private void MixColor(Color clashColor)
    {
        Color mixedColor = (_color + clashColor) / 2;
        _color = mixedColor;

        _material.SetColor("_MixColor", _color);
    }

    private void DecreaseStore()
    {
        storedLiquit -= 1/ FEAVERTIME * Time.deltaTime;
        _material.SetFloat("_Height", storedLiquit);

        if (IsEmpty())
        {
            isFeaver = false;
            storedLiquit = 0;
            feaverColor = new Color(1, 1, 1);
            _material.SetFloat("_Height", storedLiquit);
            _material.SetColor("_MixColor", _color);
        }
    }

    public void OnFeaver()
    {
        float[] colorVals = { _color.r, _color.g, _color.b };
        Color fevC = new Color(1, 1, 1);
        float tempMax = -1;
        for(int i=0; i<colorVals.Length; i++)
        {
            if(tempMax < colorVals[i])
            {
                if (i == 0)
                    fevC = new Color(1, 0, 0);
                else if (i == 1)
                    fevC = new Color(0, 1, 0);
                else if(i == 2)
                    fevC = new Color(0, 0, 1);
                else
                    fevC = new Color(1, 1, 1);

                tempMax = colorVals[i];
            }
        }
        feaverColor = fevC;
        
        _color = new Color(0, 0, 0);
        isFeaver = true;
    }
}
