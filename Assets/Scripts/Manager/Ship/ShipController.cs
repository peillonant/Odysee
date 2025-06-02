using UnityEngine;

public class ShipController : MonoBehaviour
{
    private GameObject go_Sail;

    // Variable linked to the speed of the boat
    private float f_currentSpeed;
    private float f_targetSpeed;
    private float f_startSpeed;
    private float f_TimerSpeedChange_OnWind = 0;
    private float f_TimerSpeedChange_NotOnWind = 0;
    private float f_TimerLerp = 0;
    private bool b_SailCorrectAngle;
    private float f_SpeedTransition = 0.06f;

    // Variable linked when the ship has been attacked
    private bool b_IsAttracted;
    private bool b_HasBeenTouched;
    private float f_TimerBeenTouched = 0;
    private float f_TimerIsAttracted = 0;
    private float f_DelayAttracted = 1.5f;

    // Variable linked to the Position of the boat
    private Vector3 v3_targetPosition;

    public float RetrieveTargetSpeed() => f_targetSpeed;
    public void SetTargetSpeed(float f_newTargetSpeed) => f_targetSpeed = f_newTargetSpeed;

    void Start()
    {
        go_Sail = this.transform.GetChild(2).gameObject;
        f_currentSpeed = GameConstante.F_MINSPEEDUNTOUCHED;
        f_targetSpeed = f_currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameInfo.IsGameLost() && !GameInfo.IsGameOnPause())
        {
            // PlayerMovement Method
            UpdatePlayerMovement();
            GoingForward();

            // Wind method
            WindImpact();
            UpdateSpeed();
        }
    }

    #region MovementSystem
    // Method linked to the Update of the Ship
    void UpdatePlayerMovement()
    {
        // Update the target Position to create the animation of the boat floating toward the lane
        if (GameInfo.GetCurrentRegion() == TypeRegion.OLYMPUS)
            v3_targetPosition = new(v3_targetPosition.x, v3_targetPosition.y, this.transform.position.z);
        else
            v3_targetPosition = new(v3_targetPosition.x, this.transform.position.y, this.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, v3_targetPosition, f_currentSpeed * f_SpeedTransition);

        if (b_IsAttracted)
        {
            f_TimerIsAttracted += Time.deltaTime;

            if (f_TimerIsAttracted >= f_DelayAttracted)
            {
                f_TimerIsAttracted = 0;
                b_IsAttracted = false;
            }
        }
    }

    // Method to move forward the boat and to update the distance travelled
    void GoingForward()
    {
        Vector3 v3_tmpPosition = transform.position;

        v3_tmpPosition.z += f_currentSpeed * Time.deltaTime * 6;

        transform.position = v3_tmpPosition;
    }

    // Method to update the targetPosition for the boat
    public void UpdateTargetPosition(Vector3 v3_InputMovement)
    {
        if (GameInfo.GetCurrentRegion() == TypeRegion.OLYMPUS)
        {
            // We have to update the position of the X and the Y
        }
        else if (GameInfo.GetCurrentRegion() == TypeRegion.ETNAS)
        {
            // We have to update the position with more than 3 lanes (probably 5)
        }
        else
        {
            if (!b_IsAttracted)
            {
                if (v3_InputMovement.x < 0)                                                                 // The player wants to move the boat to the left lane
                {
                    if (v3_targetPosition.x == 0)                                                           // We are on the middle lane
                        v3_targetPosition = new(-GameConstante.I_BORDERX, v3_targetPosition.y, v3_targetPosition.z);
                    else if (v3_targetPosition.x == GameConstante.I_BORDERX)                                              // We are on the right lane
                        v3_targetPosition = new(0, v3_targetPosition.y, v3_targetPosition.z);
                }
                else if (v3_InputMovement.x > 0)                                                            // The player wants to move the boat to the rigth lane
                {
                    if (v3_targetPosition.x == 0)                                                           // We are on the middle lane
                        v3_targetPosition = new(GameConstante.I_BORDERX, v3_targetPosition.y, v3_targetPosition.z);
                    else if (v3_targetPosition.x == -GameConstante.I_BORDERX)                                             // We are on the left lane
                        v3_targetPosition = new(0, v3_targetPosition.y, v3_targetPosition.z);
                }
            }
        }
    }
    #endregion

    #region Sail system
    public void TriggerRotationSail(float f_MousePositionStartZ, float f_valueReadX)
    {
        // Compute the difference between the position of the mouse when the user click it vs the current position of the mouse when the user is still hold the click
        float f_RotationVariation = (f_MousePositionStartZ - f_valueReadX) / 100;

        // Compute the current sail rotation to avoid mistake computation when the sail is on the -30°
        float f_currentSailRotation = (go_Sail.transform.rotation.eulerAngles.y > 180) ? go_Sail.transform.rotation.eulerAngles.y - 360 : go_Sail.transform.rotation.eulerAngles.y;

        float f_newSailRotation = f_currentSailRotation + f_RotationVariation;


        if (f_newSailRotation < -GameConstante.I_MAXANGLEWIND)
            f_newSailRotation = -GameConstante.I_MAXANGLEWIND;

        else if (f_newSailRotation > GameConstante.I_MAXANGLEWIND)
            f_newSailRotation = GameConstante.I_MAXANGLEWIND;

        go_Sail.transform.rotation = Quaternion.Euler(0, f_newSailRotation, 0);
    }
    #endregion


    #region Wind and Speed System 
    // Method to check if the Sail is on the correct Angle to increase the speed of the boat or not
    private void WindImpact()
    {
        // First we check if the sail is on the correct direction compare to the wind angle        
        float f_SailAngle = (go_Sail.transform.rotation.eulerAngles.y > 180) ? go_Sail.transform.rotation.eulerAngles.y - 360 : go_Sail.transform.rotation.eulerAngles.y;

        b_SailCorrectAngle = f_SailAngle >= GameInfo.GetWindAngle() - GameConstante.I_GAPANGLE && f_SailAngle <= GameInfo.GetWindAngle() + GameConstante.I_GAPANGLE;

        // Then, if the bool is true we increase the speed of the boat by one
        if (b_SailCorrectAngle)
        {
            if (f_TimerSpeedChange_NotOnWind > 0)
            {
                f_TimerSpeedChange_NotOnWind = 0;
                f_TimerSpeedChange_OnWind = 1;
            }

            f_TimerSpeedChange_OnWind += Time.deltaTime;

            if (f_TimerSpeedChange_OnWind > 1)
            {
                f_targetSpeed++;
                f_TimerSpeedChange_OnWind = 0;
            }

            if (f_targetSpeed > GameInfo.GetSpeedMax())
                f_targetSpeed = GameInfo.GetSpeedMax();
        }
        else if (!b_HasBeenTouched)   // Else, we reduce the target speed by one
        {
            if (f_TimerSpeedChange_OnWind > 0)
                f_TimerSpeedChange_OnWind = 0;

            f_TimerSpeedChange_NotOnWind += Time.deltaTime;

            if (f_TimerSpeedChange_NotOnWind > 1)
            {
                f_targetSpeed--;
                f_TimerSpeedChange_NotOnWind = 0;
            }

            if (f_targetSpeed < GameConstante.F_MINSPEEDUNTOUCHED)
                f_targetSpeed = GameConstante.F_MINSPEEDUNTOUCHED;

        }
    }

    // Method to have an acceleration or desceleration
    private void UpdateSpeed()
    {
        // Condition to manage when the ship has been touched an obstacle to generate an acceleration to the minimum Speed untouched
        if (b_HasBeenTouched && f_targetSpeed < GameConstante.F_MINSPEEDUNTOUCHED)
        {
            f_TimerBeenTouched += Time.deltaTime;

            if (f_TimerBeenTouched > 1.5f)
            {
                f_TimerBeenTouched = 0;
                f_targetSpeed++;
            }
        }
        else if (b_HasBeenTouched && f_targetSpeed >= GameConstante.F_MINSPEEDUNTOUCHED)
        {
            b_HasBeenTouched = false;
            f_TimerBeenTouched = 0;
        }

        // Condition to create the effect of acceleration and to avoid having a increae or descrease of speed to fast
        if (f_currentSpeed != f_targetSpeed)
        {
            if (f_startSpeed == 0)
                f_startSpeed = f_currentSpeed;

            f_currentSpeed = Tweening.Lerp(ref f_TimerLerp, 0.5f, f_startSpeed, f_targetSpeed);
        }
        else
        {
            f_startSpeed = 0;
        }

        GameInfo.SetCurrentSpeed(f_currentSpeed);
    }
    #endregion


    #region Ship Attacked
    // Method triggered by the ColliderController when the ship has been touched by something
    public void HasBeenTouched()
    {
        b_HasBeenTouched = true;
        f_targetSpeed = (f_currentSpeed / 2 < GameConstante.F_MINSPEEDTOUCHED) ? GameConstante.F_MINSPEEDTOUCHED : (int)Mathf.Ceil(f_currentSpeed / 2);
    }

    // Method triggered by the ColliderController when the ship has been touched by the JellyFish
    public void HasBeenTouched_JellyFish()
    {
        b_HasBeenTouched = true;
        f_targetSpeed = GameConstante.F_MINSPEEDTOUCHED;
    }

    // Method triggered by the Mermaid
    public void TriggerAttraction(float f_PositionX)
    {
        v3_targetPosition.x = f_PositionX;
        b_IsAttracted = true;
        f_TimerIsAttracted = 0;
    }
    #endregion
}
