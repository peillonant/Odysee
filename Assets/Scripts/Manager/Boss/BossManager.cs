using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject go_BossNotUsed;

    public UnityEvent endRegionFightEvent;

    float f_CurrentTimer = 0;
    float f_DelayTimer = 2f;
    
    bool b_EndRegionFightPerformed = false;
    float f_DistancePreFight = 0;

    // Variable linked to Scylla (Egee boss)
    float f_SpeedThreshold = 2;

    // Variable linked to Noise boss (Styx & Arcadia)
    float f_MaxNoise = 20;
    float f_NoiseThreshold = 12;
    float f_CurrentNoise = 0;
    float f_TimerNoiseDecrease = 0;
    float f_DelayNoise = 5;
    float f_ReductionSpeed = 1;

    //Encapsulation
    public float GetMaxNoise() => f_MaxNoise;
    public float GetNoiseThreshold() => f_NoiseThreshold;
    public float GetCurrentNoise() => f_CurrentNoise; 


    // Method called when we change the region to reset the BossManager
    public void ResetBossManager()
    {
        GameInfo.instance.SetPreFightPerformed(false);
        b_EndRegionFightPerformed = false;
        f_DistancePreFight = 0;
        f_CurrentNoise = 0;
        f_TimerNoiseDecrease = 0;
    }

    void Update()
    {
        if (!GameInfo.instance.IsBossFight() && !GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause() && GameInfo.instance.TutorielHasBeenSeen())
        {
            if (GameInfo.instance.GetDistance() < 800 && !GameInfo.instance.GetPreFightPerformed())
            {
                switch (GameInfo.instance.GetCurrentRegion().typeRegion)
                {
                    case TypeRegion.EGEE:
                        CheckBossEgee();
                        break;
                    case TypeRegion.STYX:
                        CheckBossNoise();
                        break;
                    case TypeRegion.ARCADIA:
                        CheckBossNoise();
                        break;
                }
            }
            else if (GameInfo.instance.GetDistance() > 800 && GameInfo.instance.GetDistance() - f_DistancePreFight > 150 && !b_EndRegionFightPerformed)
            {
                TriggerBoss(false);
            }
        }
    }

    #region Region Egee
    private void CheckBossEgee()
    {
        IncreaseThresholdSpeed();

        if (GameInfo.instance.GetCurrentSpeed() <= f_SpeedThreshold)
        {
            f_CurrentTimer += Time.deltaTime;

            if (f_CurrentTimer >= f_DelayTimer)
            {
                TriggerBoss(true);
                f_CurrentTimer = 0;
                f_DistancePreFight = GameInfo.instance.GetDistance();
            }
        }
        else
        {
            f_CurrentTimer = 0;
        }
    }

    // Method to increase the ThresholdSpeed for Scyllas Boss regarding the distance of the ship
    private void IncreaseThresholdSpeed()
    {
        if (GameInfo.instance.GetDistance() < 100)
            f_SpeedThreshold = 2;
        else if (GameInfo.instance.GetDistance() < 200)
            f_SpeedThreshold = 4;
        else if (GameInfo.instance.GetDistance() < 300)
            f_SpeedThreshold = 6;
        else if (GameInfo.instance.GetDistance() < 400)
            f_SpeedThreshold = 8;
        else if (GameInfo.instance.GetDistance() < 500)
            f_SpeedThreshold = 10;
        else if (GameInfo.instance.GetDistance() < 600)
            f_SpeedThreshold = 11;
        else if (GameInfo.instance.GetDistance() < 700)
            f_SpeedThreshold = 12;
        else
            f_SpeedThreshold = 13;

        GameInfo.instance.SetThresholdBoss(f_SpeedThreshold);

        // Find another way to increase the difficulty of Scylla f_SpeedThreshold *= f_ThresholdMultiplier;
    }
    #endregion

    #region Region with Noise System (Styx & Arcadia)
    private void CheckBossNoise()
    {
        if (f_CurrentNoise > 0)
        {
            f_TimerNoiseDecrease += Time.deltaTime;

            if (f_CurrentNoise >= f_NoiseThreshold)
            {
                TriggerBoss(true);
                f_TimerNoiseDecrease = 0;
                f_DistancePreFight = GameInfo.instance.GetDistance();
            }
            else if (f_TimerNoiseDecrease > f_DelayNoise)
            {
                f_CurrentNoise -= f_ReductionSpeed;
                f_TimerNoiseDecrease = 0;
            }
        }
    }

    // Method called by all element linked to the Styx region (Obstacles, Cannon, Monster) 
    public void IncreaseNoise(int i_AddNoise)
    {
        f_CurrentNoise = (f_CurrentNoise + i_AddNoise > f_MaxNoise) ? f_MaxNoise : f_CurrentNoise + i_AddNoise;
        f_TimerNoiseDecrease = 0;
    }
    #endregion

    // Method to trigger the Boss of the Region. On this method, we will know if the boss has been already created or not
    private void TriggerBoss(bool b_IsPreFight)
    {
        GameObject go_BossPrefab = GameInfo.instance.GetCurrentRegion().bossPrefab;

        // First we check if the boss has been already Used
        GameObject go_Boss = GeneralFunction.RetrieveObject(go_BossNotUsed, go_BossPrefab, this.transform);

        // If the retrieve did not work, we create the gameObject
        if (go_Boss == null)
        {
            go_Boss = Instantiate(go_BossPrefab, this.transform);
            go_Boss.name = go_BossPrefab.name;
        }

        go_Boss.GetComponent<Boss>().SetIsPreFight(b_IsPreFight);

        GameInfo.instance.SetBossFight(true);

        GameObject.Find("Sea").GetComponent<SeaManager>().RemoveObstaclesAndMonsters();

        GameInfo.instance.SetPreFightPerformed(b_IsPreFight);
        b_EndRegionFightPerformed = !b_IsPreFight;
    }


    // Method to trigger the reward regarding the defeat of the Boss
    public void TriggerReward()
    {
        if (b_EndRegionFightPerformed)
            endRegionFightEvent.Invoke();
    }


}