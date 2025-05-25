using System;
using System.Linq.Expressions;

public enum TypeRegion
{
    EGEE,
    STYX,
    OLYMPUS,
    ETNAS,
    ARCADIE,
    COUNT
}


public static class GameInfo
{
    #region Variable
    private static TypeRegion currentRegion;
    private static bool b_IsBossFight = false;
    private static float f_WindAngle;
    public static float f_SpeedMax = 20;
    private static float f_CurrentSpeed;
    private static int i_CurrentHealth = 3;
    private static int i_MaxHealth = 3;
    private static int i_CurrentScore = 0;
    private static int i_CurrentDistance = 0;
    private static int i_CurrentAmmo = 5;
    private static int i_CurrentOboles = 0;

    #endregion

    #region Encapsulation
    public static void SetCurrentRegion(TypeRegion newRegion) => currentRegion = newRegion;
    public static TypeRegion GetCurrentRegion() => currentRegion;

    /* ------------------------------------------------ */

    public static void SetBossFight(bool b_newState) => b_IsBossFight = b_newState;
    public static bool IsBossFight() => b_IsBossFight;

    /* ------------------------------------------------ */

    public static void SetWindAngle(float f_newWindAngle) => f_WindAngle = f_newWindAngle;
    public static float GetWindAngle() => f_WindAngle;

    /* ------------------------------------------------ */

    public static void SetSpeedMax(float f_newSpeedMax) => f_SpeedMax = f_newSpeedMax;
    public static float GetSpeedMax() => f_SpeedMax;

    /* ------------------------------------------------ */

    public static void SetCurrentSpeed(float f_newSpeed) => f_CurrentSpeed = f_newSpeed;
    public static float GetCurrentSpeed() => f_CurrentSpeed;

    /* ------------------------------------------------ */

    public static void DecreaseHealth(int i_nbDecrease) =>
        i_CurrentHealth = (i_CurrentHealth - i_nbDecrease > 0) ? i_CurrentHealth - i_nbDecrease : 0;
    public static void IncreaseHealth(int i_nbIncrease) =>
        i_CurrentHealth = (i_CurrentHealth + i_nbIncrease < i_MaxHealth) ? i_CurrentHealth + i_nbIncrease : i_MaxHealth;
    public static int GetCurrentHealth() => i_CurrentHealth;
    public static void SetMaxHealth(int i_newHealthMax) => i_MaxHealth = i_newHealthMax;

    /* ------------------------------------------------ */

    public static void IncreaseScore(int i_ValueToAdd) => i_CurrentScore += i_ValueToAdd;
    public static int GetScore() => i_CurrentScore;

    /* ------------------------------------------------ */

    public static void IncreaseDistance() => i_CurrentDistance++;
    public static int GetDistance() => i_CurrentDistance;

    /* ------------------------------------------------ */

    public static void SetNbAmmo(int i_newNbAmmo) => i_CurrentAmmo = i_newNbAmmo;
    public static void DescreaseAmmo() => i_CurrentAmmo--;
    public static void IncreaseAmmo()
    {
        if (i_CurrentAmmo < 10)
            i_CurrentAmmo++;
    }
    public static int GetNbAmmo() => i_CurrentAmmo;

    /* ------------------------------------------------ */

    public static void IncreaseCoin(int i_newCoin) => i_CurrentOboles += i_newCoin;
    public static void DecreaseCoin(int i_newCoin) =>
        i_CurrentOboles = (i_CurrentOboles - i_newCoin < 0) ? 0 : i_CurrentOboles - i_newCoin;
    public static int GetCurrentOboles() => i_CurrentOboles;

    /* ------------------------------------------------ */
    #endregion
}
