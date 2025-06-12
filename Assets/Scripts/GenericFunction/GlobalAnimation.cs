using UnityEngine;

public class GlobalAnimation
{
    public static Vector3 AnimationFloating(ref float f_TimerAnime, float f_DelayAnime, float f_BottomPos, float f_TopPos, Vector3 localPosition)
    {
        f_TimerAnime += Time.deltaTime;
        float t = Mathf.PingPong(f_TimerAnime / f_DelayAnime, 1f);
        float f_easedT = Tweening.InOutQuad(t);

        // Interpolation between the top position and bottom
        Vector3 newPosition = localPosition;
        newPosition.y = Mathf.Lerp(f_BottomPos, f_TopPos, f_easedT);

        return newPosition;
    }
}