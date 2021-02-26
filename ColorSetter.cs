using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private static float intensity = Mathf.Pow(2, 2.5f);

    private static Color redColor = new Color(1 * (intensity-1f), 0, 0);
    private static Color greenColor = new Color(0, 1 * (intensity-2f), 0);
    private static Color blueColor = new Color(0, 0.1f * intensity, 1 * intensity);

    public static Color GetRedColor()
    {
        return redColor;
    }
    public static Color GetGreenColor()
    {
        return greenColor;
    }
    public static Color GetBlueColor()
    {
        return blueColor;
    }

    public static string GetColorType(Color color)
    {
        float[] colorVals = { color.r, color.g, color.b };
        string colorType = null;
        float tempMax = -1;
        for (int i = 0; i < colorVals.Length; i++)
        {
            if (tempMax < colorVals[i])
            {
                if (i == 0)
                    colorType = "Red";
                else if (i == 1)
                    colorType = "Green";
                else if (i == 2)
                    colorType = "Blue";
                else
                    colorType = null;

                tempMax = colorVals[i];
            }
        }

        return colorType;
    }

    public static Color GetColor(string colorType)
    {
        switch (colorType)
        {
            case "Red":
                return redColor;
            case "Green":
                return greenColor;
            case "Blue":
                return blueColor;
            default:
                return new Color(0, 0, 0);
        }
    }
}
