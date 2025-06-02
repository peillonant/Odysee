using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WindManager : MonoBehaviour
{
    [SerializeField] GameObject go_Pennant;
    [SerializeField] GameObject go_Wind;
    [SerializeField] List<GameObject> go_WindPrefabs;
    
    private float f_TimerChangeWindAngle = 0;
    private float f_DelayChangeWindAngle;
    private float f_TimerDisplayWindEffect = 0;

    private float f_TimerUpdateAngle = 0;
    private float f_LerpDurationAngle = 3;
    private float f_CurrentWindAngle = 0;
    private float f_LerpValueStart = 0;
    private float f_TargetWindAngle = 0;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameInfo.IsGameLost() && !GameInfo.IsGameOnPause())
        {
            // Check if the delay to change the Wind need to be changed
            UpdateDelayToChangeWind();

            // Then, with the delay computed, do we have to update the angle of the wind
            CheckTimer();

            // Change the display of the wind (rotation angle and the pennant)
            UpdateAngleWind();

            // Add on the scene Wind effect to add more interaction
            DisplayWindEffect();
        }
    }

    // Method to update the delay timer to change the wind regarding the region and the speed of the ship
    private void UpdateDelayToChangeWind()
    {
        if (GameInfo.GetCurrentRegion() == TypeRegion.EGEE)
        {
            float f_ShipSpeed = GameInfo.GetCurrentSpeed();

            if (f_ShipSpeed == 10)
                f_DelayChangeWindAngle = 6;
            else if (f_ShipSpeed > 10 && f_ShipSpeed <= 15)
                f_DelayChangeWindAngle = 5;
            else if (f_ShipSpeed > 15 && f_ShipSpeed <= 20)
                f_DelayChangeWindAngle = 4;
            else if (f_ShipSpeed > 20 && f_ShipSpeed <= 25)
                f_DelayChangeWindAngle = 3; 
            else if (f_ShipSpeed > 25)
                f_DelayChangeWindAngle = 2;    
        }
    }

    // Method to check the Timer to trigger the change of the Wind Angle
    private void CheckTimer()
    {
        f_TimerChangeWindAngle += Time.deltaTime;

        if (f_TimerChangeWindAngle >= f_DelayChangeWindAngle)
        {
            f_TimerChangeWindAngle = 0;
            ChangewindAngle();
        }
    }   

    // Method that create a new TargetAngle for the wind
    private void ChangewindAngle()
    {
        float f_newAngle = Random.Range(-GameConstante.I_MAXANGLEWIND, GameConstante.I_MAXANGLEWIND);

        int cpt = 0;
        int maxcpt = 100;

        while (f_newAngle >= f_CurrentWindAngle - GameConstante.I_GAPANGLE && f_newAngle <= f_CurrentWindAngle + GameConstante.I_GAPANGLE)
        {
            f_newAngle = Random.Range(-GameConstante.I_MAXANGLEWIND, GameConstante.I_MAXANGLEWIND);

            cpt++;
            if (cpt > maxcpt)
            {
                f_newAngle = GameConstante.I_MAXANGLEWIND;
                break;
            }
        }

        f_TargetWindAngle = f_newAngle;
        f_LerpValueStart = f_CurrentWindAngle;
    }

    // Method that update the angle of the wind between the currentAngle to the targetOne and Update the angle of the Pennant at the top of the Ship
    private void UpdateAngleWind()
    {
        if (f_CurrentWindAngle != f_TargetWindAngle)
        {
            // First we compute the lerp to extrapolate the currentWindAngle to the Target one
            f_CurrentWindAngle = Tweening.Lerp(ref f_TimerUpdateAngle, f_LerpDurationAngle, f_LerpValueStart, f_TargetWindAngle);

            // Second, we update the angle of the Pennant to gave to the player a clear view of the wind
            UpdatePennantRotation();

            // Third, we send the information to the GameInfo to give access to this information everywhere else on the code
            GameInfo.SetWindAngle(f_CurrentWindAngle);
        }
    }

    // Methode to Update the rotation of the Pennant
    private void UpdatePennantRotation()
    {
        float f_newPennantAngle = GameConstante.F_ANGLEPENNANTDEFAULT + f_CurrentWindAngle;

        // Need to check to have the correct way to update the rotation of the transform
        go_Pennant.transform.rotation = Quaternion.Euler(0, f_newPennantAngle, 0);
    }

    // Method that create the GameObject Wind Particle around the boat with the angle of the wind
    private void DisplayWindEffect()
    {
        f_TimerDisplayWindEffect += Time.deltaTime;

        if (f_TimerDisplayWindEffect > 4)
        {
            int i_indexWindParent = Random.Range(0, go_Wind.transform.childCount);

            int i_indexWindPrefab = Random.Range(0, go_WindPrefabs.Count);

            GameObject newWindEffect = Instantiate(go_WindPrefabs[i_indexWindPrefab], go_Wind.transform.GetChild(i_indexWindParent));
            newWindEffect.transform.localRotation = Quaternion.Euler(0, f_TargetWindAngle, 0);

            f_TimerDisplayWindEffect = 0;
        }
    }
}
