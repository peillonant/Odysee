using System.Numerics;
using UnityEngine;

public static class Tweening
{
    public static float Lerp(ref float f_Timer, float f_Duration, float f_Start, float f_Target)
    {
        if (f_Timer < f_Duration)
        {
            var result = Mathf.Lerp(f_Start, f_Target, f_Timer / f_Duration);
            f_Timer += Time.deltaTime;
            return result;
        }
        else
        {
            f_Timer = 0;
            return f_Target;
        }
    }

    public static float InOutBack(float t)
    {
        if (t < 0.5f)
        {
            float a = 2 * t;
            return (a * a * a - a * Mathf.Sin(a * Mathf.PI)) / 2;
        }
        else
        {
            float a = -2 * t + 2;
            return 1 - (a * a * a - a * Mathf.Sin(a * Mathf.PI)) / 2;
        }
    }
    
    public static float InOutQuad(float t)
    {
            return (t < 0.5f)
                ? (2 * t * t)
                : (1 - Mathf.Pow(-2 * t + 2, 2) / 2);
    }
}