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

    public static Color LerpColor(Color start, Color target, float f_Timer, float f_Delay)
    {
        Color result = new();
        result.r = Mathf.Lerp(start.r, target.r, f_Delay - f_Timer);
        result.g = Mathf.Lerp(start.g, target.g, f_Delay - f_Timer);
        result.b = Mathf.Lerp(start.b, target.b, f_Delay - f_Timer);
        return result;
    }
}