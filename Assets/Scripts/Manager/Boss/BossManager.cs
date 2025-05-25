using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> go_BossPrefabList;
    [SerializeField] private GameObject go_BossParent;
    [SerializeField] private GameObject go_BossNotUsed;

    float f_CurrentTimer = 0;
    float f_DelayTimer = 2f;
    bool b_PreFightPerformed = false;
    float f_DistancePreFight = 0;

    // Variable linked to Scylla (Egee boss)
    [SerializeField] float f_SpeedThreshold = 2;

    // Variable linked to the multiplier of Boss, increasing when you already pass by the region several times
    float f_ThresholdMultiplier = 1;

    // Method called when we change the region to reset the BossManager
    public void ResetPrefightPerformed()
    {
        b_PreFightPerformed = false;
        f_DistancePreFight = 0;
    }

    void Update()
    {
        if (!GameInfo.IsBossFight())
        {
            if (GameInfo.GetDistance() < 800 && !b_PreFightPerformed)
            {
                switch (GameInfo.GetCurrentRegion())
                {
                    case TypeRegion.EGEE:
                        CheckBossEgee();
                        break;
                }
            }
            else if (GameInfo.GetDistance() - f_DistancePreFight > 150)
            {
                switch (GameInfo.GetCurrentRegion())
                {
                    case TypeRegion.EGEE:
                        TriggerBoss(false, go_BossPrefabList[0]);
                        break;
                }
            }
        }
    }

    #region Region Egee
    private void CheckBossEgee()
    {
        IncreaseThresholdSpeed();

        if (GameInfo.GetCurrentSpeed() <= f_SpeedThreshold)
        {
            f_CurrentTimer += Time.deltaTime;

            if (f_CurrentTimer >= f_DelayTimer)
            {
                TriggerBoss(true, go_BossPrefabList[0]);
                f_CurrentTimer = 0;
                f_DistancePreFight = GameInfo.GetDistance();
            }
        }
        else
        {
            f_CurrentTimer = 0;
        }
    }

    private void IncreaseThresholdSpeed()
    {
        if (GameInfo.GetDistance() < 100)
            f_SpeedThreshold = 2;
        else if (GameInfo.GetDistance() < 200)
            f_SpeedThreshold = 4;
        else if (GameInfo.GetDistance() < 300)
            f_SpeedThreshold = 6;
        else if (GameInfo.GetDistance() < 400)
            f_SpeedThreshold = 8;
        else if (GameInfo.GetDistance() < 500)
            f_SpeedThreshold = 10;
        else if (GameInfo.GetDistance() < 600)
            f_SpeedThreshold = 12;
        else if (GameInfo.GetDistance() < 700)
            f_SpeedThreshold = 14;
        else
            f_SpeedThreshold = 16;

        f_SpeedThreshold *= f_ThresholdMultiplier;
    }
    #endregion

    private void TriggerBoss(bool b_IsPreFight, GameObject go_BossPrefab)
    {
        // First we check if the Scylla has been already Used
        GameObject go_Boss = GeneralFunction.RetrieveObject(go_BossNotUsed, go_BossPrefab, go_BossParent.transform);

        // If the retrieve did not work, we create the gameObject
        if (go_Boss == null)
        {
            go_Boss = Instantiate(go_BossPrefab, go_BossParent.transform);
            go_Boss.name = go_BossPrefab.name;
        }
        else
        {
            if (!go_Boss.GetComponent<Boss>().IsPreFight())
                go_Boss.GetComponent<Boss>().IncreaseHealth();
        }

        go_Boss.GetComponent<Boss>().SetIsPreFight(b_IsPreFight);

        GameInfo.SetBossFight(true);

        GameObject.Find("Sea").GetComponent<SeaManager>().RemoveObstaclesAndMonsters();
    }
    
}