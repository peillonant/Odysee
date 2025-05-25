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
}