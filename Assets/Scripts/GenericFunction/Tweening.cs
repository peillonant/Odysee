using System.Numerics;

public static class Tweening
{
    public static float LerpFloat (float f_start, float f_end, float f_timer)
    {
        return f_start + (f_end - f_start) * f_timer;
    }
}