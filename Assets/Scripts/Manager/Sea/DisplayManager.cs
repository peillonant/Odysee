using UnityEngine;

// Class that will manage the change color of all element of the sea (and also the camera and fog) regarding the current region
public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private float f_TimerSea = 0;
    private float f_TimerLane = 0;
    private float f_TimerCamera = 0;
    private float f_DelayUpdate = 10;

    void Update()
    {
        // CheckColorSea();

        // CheckColorCamera();

        // CheckColorFog();
    }

    public void ResetTimer()
    {
        f_TimerSea = 0;
        f_TimerLane = 0;
        f_TimerCamera = 0;
    }

    private void CheckColorSea()
    {
        GameObject sea = this.transform.GetChild(0).GetChild(0).gameObject;
        GameObject lane = this.transform.GetChild(0).GetChild(1).gameObject;
        
        f_TimerSea += Time.deltaTime;

        if (sea.GetComponent<Renderer>().sharedMaterial.color != GameInfo.instance.GetCurrentRegion().seaColor)
        {
            Debug.Log("Not same color => " + f_TimerSea + " // " + sea.GetComponent<Renderer>().sharedMaterial.color + " vs " + GameInfo.instance.GetCurrentRegion().seaColor);

            if (f_TimerSea < f_DelayUpdate)
            {
                sea.GetComponent<Renderer>().sharedMaterial.color = Tweening.LerpColor(sea.GetComponent<Renderer>().sharedMaterial.color, GameInfo.instance.GetCurrentRegion().seaColor, f_TimerSea, f_DelayUpdate);
            }
            else
            {
                sea.GetComponent<Renderer>().sharedMaterial.color = GameInfo.instance.GetCurrentRegion().seaColor;
            }
        }
        else
        {
            Debug.Log("Did not worked");
        }

        if (lane.GetComponent<Renderer>().sharedMaterial.color != GameInfo.instance.GetCurrentRegion().laneColor)
        {
            if (f_TimerSea < f_DelayUpdate)
            {
                lane.GetComponent<Renderer>().sharedMaterial.color = Tweening.LerpColor(lane.GetComponent<Renderer>().sharedMaterial.color, GameInfo.instance.GetCurrentRegion().laneColor, f_TimerSea, f_DelayUpdate);
            }
            else
            {
                lane.GetComponent<Renderer>().sharedMaterial.color = GameInfo.instance.GetCurrentRegion().laneColor;
            }
        }
    }

    private void CheckColorCamera()
    {

    }

    private void CheckColorFog()
    {

    }

}