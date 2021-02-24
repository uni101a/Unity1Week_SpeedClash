using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStore : MonoBehaviour
{
    private Material _material;
    private float storedLiquit = 0;
    private Color _color = new Color(0, 0, 0);

    private int MaxMixingNum = 100;
    private int mixedNum = 0;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
    }

    public void Store(Color clashColor)
    {
        UpLiquid();
        MixColor(clashColor);
    }

    private void UpLiquid()
    {
        storedLiquit += 1.0f / MaxMixingNum;
        if (storedLiquit >= 1.0f)
            return;
        _material.SetFloat("_Height", storedLiquit);
    }

    private void MixColor(Color clashColor)
    {
        mixedNum++;

        Color mixedColor = (_color + clashColor) / 2;
        _color = mixedColor;

        _material.SetColor("_MixColor", _color);
    }
}
