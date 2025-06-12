using UnityEngine;

public class DistanceController : MonoBehaviour
{
    private float f_startDistanceComputation = 0;
    private float f_StepDistanceToSpeed = GameConstante.I_GAPDISTANCESPEED;

    // Update is called once per frame
    void Update()
    {
        if (!GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            UpdateDistanceValue();
            IncreaseSpeedMax();
        }
    }

    // Method used to compute the Distance regarding the position Z of the ship
    private void UpdateDistanceValue()
    {
        if (!GameInfo.instance.IsBossFight())
        {
            if (this.transform.position.z - f_startDistanceComputation > 10)
            {
                // We add 1 to the distance everytime we move of 10 units on the Z position
                GameInfo.instance.IncreaseDistance();
                f_startDistanceComputation = this.transform.position.z;

                // We also add 1 * Multiplier to the Score everytime we move of 10 units on the Z position;
                int multiplier = 1;

                float f_thresholdMultiplier  = (( GameInfo.instance.GetSpeedMax() - GameConstante.F_MINSPEEDUNTOUCHED ) / 2) + GameConstante.F_MINSPEEDUNTOUCHED;

                if (GameInfo.instance.GetCurrentSpeed() < f_thresholdMultiplier)
                    multiplier = 1;
                else if (GameInfo.instance.GetCurrentSpeed() < GameInfo.instance.GetSpeedMax())
                    multiplier = 2;
                else if (GameInfo.instance.GetCurrentSpeed() == GameInfo.instance.GetSpeedMax())
                    multiplier = 3;

                GameInfo.instance.IncreaseScoreDistance(multiplier);
            }
        }
    }

    // Methode that increase the speedMax regarding the distance covered during the all run
    void IncreaseSpeedMax()
    {
        if (GameInfo.instance.GetSpeedMax() < 50)
        {
            if (GameInfo.instance.GetAllDistanceCovered() > f_StepDistanceToSpeed)
            {
                int i_newSpeedMax = (int) Mathf.Ceil(GameInfo.instance.GetSpeedMax() * 1.1f);
                GameInfo.instance.SetSpeedMax(i_newSpeedMax);
                f_StepDistanceToSpeed += GameConstante.I_GAPDISTANCESPEED;
            }
        }
    }
}
