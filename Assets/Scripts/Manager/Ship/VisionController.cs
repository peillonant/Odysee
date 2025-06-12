using UnityEngine;

public class VisionController : MonoBehaviour
{

    private bool b_FogChanging = false;
    private bool b_FogClosedShip = false;
    private bool b_FogBackToNormal = false;
    private float f_TimerChange = 0;
    private float f_DelayChange = 1;

    public void SetFogChanging(float f_newDelayChange)
    {
        b_FogChanging = true;
        f_DelayChange = f_newDelayChange;
    }

    void Update()
    {
        if (b_FogChanging)
        {
            ManageFog();
        }
    }

    private void ManageFog()
    {
        if (!b_FogClosedShip && !b_FogBackToNormal)
        {
            f_TimerChange += Time.deltaTime;

            RenderSettings.fogStartDistance = Mathf.Lerp(150, 0, f_TimerChange / (f_DelayChange / 2));
            RenderSettings.fogEndDistance = Mathf.Lerp(700, 350, f_TimerChange / (f_DelayChange / 2));

            if (f_TimerChange > (f_DelayChange / 2))
            {
                f_TimerChange = 0;
                b_FogClosedShip = true;
            }
        }
        else if (b_FogClosedShip && !b_FogBackToNormal)
        {
            f_TimerChange += Time.deltaTime;

            if (f_TimerChange > f_DelayChange)
            {
                f_TimerChange = 0;
                b_FogBackToNormal = true;
                b_FogClosedShip = false;
            }
        }
        else if (b_FogBackToNormal)
        {
            f_TimerChange += Time.deltaTime;

            RenderSettings.fogStartDistance = Mathf.Lerp(0, 150, f_TimerChange / (f_DelayChange / 2));
            RenderSettings.fogEndDistance = Mathf.Lerp(350, 700, f_TimerChange / (f_DelayChange / 2));

            if (f_TimerChange > (f_DelayChange / 2))
            {
                f_TimerChange = 0;
                b_FogBackToNormal = false;
                b_FogChanging = false;
            }
        }
    }

}