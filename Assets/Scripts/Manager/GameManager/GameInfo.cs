using System;
using System.Collections.Generic;
using UnityEngine;

public enum TypeRegion
{
    EGEE,
    STYX,
    ARCADIA,
    COUNT
}


public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;

    // Launch the persistence of the gameObject
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;

        Application.targetFrameRate = 60;
    }

    #region Event & Action
    public Action TriggerLost;
    public Action TriggerOnPause;
    #endregion

    #region Variable
    [SerializeField] private List<RegionScriptableObject> listRegion;
    [SerializeField] private RegionScriptableObject currentRegion;
    [SerializeField] private RegionScriptableObject previousRegion;
    private bool b_IsBossFight = false;
    private bool b_PreFightPerformed = false;
    private bool b_GameLost = false;
    private bool b_GameOnPause = false;
    private bool b_OntheWind = true;
    private bool b_TutorielSeen = false;
    private float f_WindAngle;
    private float f_SailAngle;
    private float f_SpeedMax = 20;
    private float f_CurrentSpeed;
    private float f_ThresholdBoss = 0;
    private int i_CurrentHealth = 3;
    private int i_MaxHealth = 3;
    private int i_CurrentScore = 0;
    private int i_CurrentMultiplier = 1;
    private int i_CurrentDistance = 0;
    private int i_AllDistanceCovered = 0;
    private int i_CurrentAmmo = 5;
    private int i_CurrentOboles = 0;
    private List<TypeRegion> rewardListCollected = new(); 
    #endregion

    #region Encapsulation
    public void SetCurrentRegion(RegionScriptableObject newRegion) => currentRegion = newRegion;
    public RegionScriptableObject GetCurrentRegion() => currentRegion;

    public void SetPreviousRegion() => previousRegion = currentRegion;
    public RegionScriptableObject GetPreviousRegion() => previousRegion;

    public List<RegionScriptableObject> GetListRegion() => listRegion;

    /* ------------------------------------------------ */

    public void SetBossFight(bool b_newState) => b_IsBossFight = b_newState;
    public bool IsBossFight() => b_IsBossFight;

    /* ------------------------------------------------ */

    public void SetWindAngle(float f_newWindAngle) => f_WindAngle = f_newWindAngle;
    public float GetWindAngle() => f_WindAngle;

    /* ------------------------------------------------ */

    public void SetSailAngle(float f_newSailAngle) => f_SailAngle = f_newSailAngle;
    public float GetSailAngle() => f_SailAngle;

    /* ------------------------------------------------ */

    public void SetOnWind(bool f_state) => b_OntheWind = f_state;
    public bool IsOnWind() => b_OntheWind;

    /* ------------------------------------------------ */

    public void SetSpeedMax(float f_newSpeedMax) => f_SpeedMax = f_newSpeedMax;
    public float GetSpeedMax() => f_SpeedMax;

    /* ------------------------------------------------ */

    public void SetCurrentSpeed(float f_newSpeed) => f_CurrentSpeed = f_newSpeed;
    public float GetCurrentSpeed() => f_CurrentSpeed;

    /* ------------------------------------------------ */

    public void DecreaseHealth(int i_nbDecrease)
    {
        i_CurrentHealth = (i_CurrentHealth - i_nbDecrease > 0) ? i_CurrentHealth - i_nbDecrease : 0;

        if (i_CurrentHealth == 0)
        {
            b_GameLost = true;
            TriggerLost.Invoke();
        }
    }
    public void IncreaseHealth(int i_nbIncrease) =>
        i_CurrentHealth = (i_CurrentHealth + i_nbIncrease < i_MaxHealth) ? i_CurrentHealth + i_nbIncrease : i_MaxHealth;
    public int GetCurrentHealth() => i_CurrentHealth;
    public void SetMaxHealth(int i_newHealthMax) => i_MaxHealth = i_newHealthMax;

    /* ------------------------------------------------ */

    public void IncreaseScore(int i_ValueToAdd)
    {
        i_CurrentScore += i_ValueToAdd;
        HUDManager.instance.AddScoreDisplay(i_ValueToAdd);
    }

    public void DeacreaseScore(int i_ValueToRemove)
    {
        i_CurrentScore = (i_CurrentScore - i_ValueToRemove < 0) ? 0 : i_CurrentScore - i_ValueToRemove;
        HUDManager.instance.AddScoreDisplay(-i_ValueToRemove);
    }
    public int GetScore() => i_CurrentScore;
    public void IncreaseScoreDistance(int i_newMultiplier)
    {
        if (i_newMultiplier != i_CurrentMultiplier)
            i_CurrentMultiplier = i_newMultiplier;

        i_CurrentScore += 1 * i_CurrentMultiplier;
    }

    /* ------------------------------------------------ */

    public int GetMultiplier() => i_CurrentMultiplier;

    /* ------------------------------------------------ */

    public void IncreaseDistance()
    {
        i_CurrentDistance++;
        i_AllDistanceCovered++;
    }
    public int GetDistance() => i_CurrentDistance;
    public void ResetDistance() => i_CurrentDistance = 0;
    public int GetAllDistanceCovered() => i_AllDistanceCovered;

    /* ------------------------------------------------ */

    public void SetNbAmmo(int i_newNbAmmo) => i_CurrentAmmo = i_newNbAmmo;
    public void DescreaseAmmo() => i_CurrentAmmo--;
    public void IncreaseAmmo()
    {
        if (i_CurrentAmmo < 10)
            i_CurrentAmmo++;
    }
    public int GetNbAmmo() => i_CurrentAmmo;

    /* ------------------------------------------------ */

    public void IncreaseCoin(int i_newCoin) => i_CurrentOboles += i_newCoin;
    public void DecreaseCoin(int i_newCoin) =>
        i_CurrentOboles = (i_CurrentOboles - i_newCoin < 0) ? 0 : i_CurrentOboles - i_newCoin;
    public int GetCurrentOboles() => i_CurrentOboles;

    /* ------------------------------------------------ */

    public bool IsGameLost() => b_GameLost;
    public bool IsGameOnPause() => b_GameOnPause;
    public void TriggerPauseMenu()
    {
        b_GameOnPause = !b_GameOnPause;
        TriggerOnPause.Invoke();
    }

    /* ------------------------------------------------ */

    public void SetTutorielSeen(bool b_newState) => b_TutorielSeen = b_newState;
    public bool TutorielHasBeenSeen() => b_TutorielSeen;

    /* ------------------------------------------------ */

    public void SetThresholdBoss(float f_NewThreshold) => f_ThresholdBoss = f_NewThreshold;
    public float GetThresholdBoss() => f_ThresholdBoss;

    /* ------------------------------------------------ */

    public void SetPreFightPerformed(bool b_state) => b_PreFightPerformed = b_state;
    public bool GetPreFightPerformed() => b_PreFightPerformed;


    // Update this method when we create more region to have a boost of score when collected all reward linked to region
    public void AddRewardToList() => rewardListCollected.Add(currentRegion.typeRegion);

    public List<TypeRegion> GetRewardList() => rewardListCollected;
    #endregion
}
