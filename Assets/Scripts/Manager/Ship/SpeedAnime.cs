using UnityEngine;

public class SpeedAnime : MonoBehaviour
{
    [SerializeField] Camera go_Camera;
    [SerializeField] Transform go_Sail;

    private float f_FOVMin_Hit = 60;
    private float f_FOVMin_Wind = 65;
    private float f_FOVMax = 85;
    private float f_SailSizeZMin_Hit = 0.35f;
    private float f_SailSizeZMin_Wind = 0.75f;
    private float f_SailSizeZMax = 1.25f;

    void Update()
    {
        UpdateFOVCamera();

        UpdateSail();
    }

    private void UpdateFOVCamera()
    {
        float f_currentSpeed = GameInfo.instance.GetCurrentSpeed();

        if (f_currentSpeed < 10)
        {
            float ratioFOV = f_currentSpeed / GameConstante.F_MINSPEEDUNTOUCHED;
            go_Camera.fieldOfView = f_FOVMin_Hit + ((f_FOVMin_Wind - f_FOVMin_Hit) * ratioFOV);
        }
        else
        {
            float ratioFOV = (f_currentSpeed - GameConstante.F_MINSPEEDUNTOUCHED) / (GameInfo.instance.GetSpeedMax() - GameConstante.F_MINSPEEDUNTOUCHED);
            go_Camera.fieldOfView = f_FOVMin_Wind + ((f_FOVMax - f_FOVMin_Wind) * ratioFOV);
        }
    }

    private void UpdateSail()
    {
        float f_currentSpeed = GameInfo.instance.GetCurrentSpeed();
        Vector3 localScale = go_Sail.localScale;

        if (f_currentSpeed < 10)
        {
            float ratioSizeZ = f_currentSpeed / GameConstante.F_MINSPEEDUNTOUCHED;
            localScale.z = f_SailSizeZMin_Hit + ((f_SailSizeZMin_Wind - f_SailSizeZMin_Hit) * ratioSizeZ);
        }
        else
        {
            float ratioSizeZ = (f_currentSpeed - GameConstante.F_MINSPEEDUNTOUCHED) / (GameInfo.instance.GetSpeedMax() - GameConstante.F_MINSPEEDUNTOUCHED);
            localScale.z = f_SailSizeZMin_Wind + ((f_SailSizeZMax - f_SailSizeZMin_Wind) * ratioSizeZ);
        }

        go_Sail.localScale = localScale;
    }
}