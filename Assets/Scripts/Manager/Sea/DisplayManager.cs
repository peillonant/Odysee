using UnityEngine;

// Class that will manage the change color of all element of the sea (and also the camera and fog) regarding the current region
public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Material materialSea;
    private Material materialLane;
    

    private float f_TimerUpdate = 0;
    private float f_DelayUpdate = 5;

    private bool b_ColorToChange = false;

    void Start()
    {
        materialSea = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().sharedMaterial;
        materialLane = transform.GetChild(0).GetChild(1).GetComponent<Renderer>().sharedMaterial;

        ResetColor();
    }

    void ResetColor()
    {
        materialSea.color = GameInfo.instance.GetCurrentRegion().seaColor;
        materialLane.color = GameInfo.instance.GetCurrentRegion().laneColor;
        mainCamera.backgroundColor = GameInfo.instance.GetCurrentRegion().cameraColor;
        RenderSettings.fogColor = GameInfo.instance.GetCurrentRegion().cameraColor;
    }

    void Update()
    {
        if (b_ColorToChange)
        {
            f_TimerUpdate += Time.deltaTime;

            CheckColorSea();

            CheckColorCamera();

            CheckColorFog();


            if (f_TimerUpdate > f_DelayUpdate)
                b_ColorToChange = false;
        }
    }

    public void TriggerChangeDisplay()
    {
        f_TimerUpdate = 0;
        b_ColorToChange = true;
    }

    private void CheckColorSea()
    {
        if (materialSea.color != GameInfo.instance.GetCurrentRegion().seaColor)
        {
            materialSea.color = Color.Lerp(GameInfo.instance.GetPreviousRegion().seaColor, GameInfo.instance.GetCurrentRegion().seaColor, f_TimerUpdate / f_DelayUpdate);
        }

        if (materialLane.color != GameInfo.instance.GetCurrentRegion().laneColor)
        {
            materialLane.color = Color.Lerp(GameInfo.instance.GetPreviousRegion().laneColor, GameInfo.instance.GetCurrentRegion().laneColor, f_TimerUpdate / f_DelayUpdate);
        }
    }

    private void CheckColorCamera()
    {
        if (mainCamera.backgroundColor != GameInfo.instance.GetCurrentRegion().cameraColor)
        {
            mainCamera.backgroundColor = Color.Lerp(GameInfo.instance.GetPreviousRegion().cameraColor, GameInfo.instance.GetCurrentRegion().cameraColor, f_TimerUpdate / f_DelayUpdate);
        }
    }

    private void CheckColorFog()
    {
        if (RenderSettings.fogColor != GameInfo.instance.GetCurrentRegion().cameraColor)
        {
            RenderSettings.fogColor = Color.Lerp(GameInfo.instance.GetPreviousRegion().cameraColor, GameInfo.instance.GetCurrentRegion().cameraColor, f_TimerUpdate / f_DelayUpdate);
        }
    }

}