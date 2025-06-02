using UnityEngine;

public class DistanceController : MonoBehaviour
{
    private float f_startDistanceComputation = 0;
    private float f_StepDistanceToSpeed = GameConstante.I_GAPDISTANCESPEED;

    // Update is called once per frame
    void Update()
    {
        if (!GameInfo.IsGameLost() && !GameInfo.IsGameOnPause())
        {
            UpdateDistanceValue();
            IncreaseSpeedMax();
        }
    }

    // Method used to compute the Distance regarding the position Z of the ship
    private void UpdateDistanceValue()
    {
        if (!GameInfo.IsBossFight())
        {
            if (this.transform.position.z - f_startDistanceComputation > 10)
            {
                // We add 1 to the distance everytime we move of 10 units on the Z position
                GameInfo.IncreaseDistance();
                f_startDistanceComputation = this.transform.position.z;

                // We also 1 to the Score everytime we move of 10 units on the Z position;
                GameInfo.IncreaseScore(1);
            }
        }
    }

    void IncreaseSpeedMax()
    {
        if (GameInfo.GetSpeedMax() < 50)
        {
            if (GameInfo.GetDistance() > f_StepDistanceToSpeed)
            {
                int i_newSpeedMax = (int) Mathf.Ceil(GameInfo.GetCurrentSpeed() * 1.1f);
                GameInfo.SetSpeedMax(i_newSpeedMax);
                f_StepDistanceToSpeed += GameConstante.I_GAPDISTANCESPEED;
            }
        }
    }
}
