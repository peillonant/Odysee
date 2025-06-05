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

                // We also 1 to the Score everytime we move of 10 units on the Z position;

                // UPDATE THE COMPUTATION OF THE SCORE
                /*  Le multiplicateur suit une montée fluide entre 1x et 3x, selon la formule :
                    Multiplicateur = 1 + ((vitesse - 7) / (30 - 7)) * 2
                    ● Entre 1 et 7 m/s, le multiplicateur reste à 1x.
                    ● Au-delà de 7 m/s, la vitesse commence à influencer directement les
                    points gagnés.
                    À la vitesse maximale
                */

                GameInfo.instance.IncreaseScore(1);
            }
        }
    }

    void IncreaseSpeedMax()
    {
        if (GameInfo.instance.GetSpeedMax() < 50)
        {
            if (GameInfo.instance.GetDistance() > f_StepDistanceToSpeed)
            {
                int i_newSpeedMax = (int) Mathf.Ceil(GameInfo.instance.GetCurrentSpeed() * 1.1f);
                GameInfo.instance.SetSpeedMax(i_newSpeedMax);
                f_StepDistanceToSpeed += GameConstante.I_GAPDISTANCESPEED;
            }
        }
    }
}
