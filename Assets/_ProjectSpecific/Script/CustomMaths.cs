using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMaths
{
    public static float Remaping(float i_From, float i_FromMin, float i_FromMax, float i_ToMin, float i_ToMax)
    {
        float fromAbs = i_From - i_FromMin;
        float fromMaxAbs = i_FromMax - i_FromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = i_ToMax - i_ToMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + i_ToMin;

        return to;
    }

    public static string HideBigNumber(int i_Num)
    {
        if (i_Num >= 100000000)
        {
            return (i_Num / 1000000D).ToString("0.#M");
        }
        if (i_Num >= 1000000)
        {
            return (i_Num / 1000000D).ToString("0.##M");
        }
        if (i_Num >= 100000)
        {
            return (i_Num / 1000D).ToString("0.#k");
        }
        if (i_Num >= 10000)
        {
            return (i_Num / 1000D).ToString("0.##k");
        }
        if(i_Num >= 1000)
        {
            return (i_Num / 1000D).ToString("0.#k");
        }

        return i_Num.ToString();
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        var fraction = timeToDisplay * 1000;
        fraction = fraction % 1000;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

public static class InGameExtension
{
    public static void ShuffleList<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


