using UnityEngine;

public class GameConstante : MonoBehaviour
{
    public static GameConstante instance;

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
    }


    // All const variable linked to the Boat control
    public const float I_MAXANGLEWIND = 30;
    public const int I_GAPANGLE = 5;
    public const float F_MINSPEEDUNTOUCHED = 10;
    public const float F_MINSPEEDTOUCHED = 5;

    // Constant of the middle of the Lane on the Left and on the Right (Use for the Ship, obstacle, enemies and boss)
    public const int I_BORDERX = 35;

    // Const used by the SeaManager
    public const int I_GAPBEFOREREMOVING = 70;
    public const int I_SIZESEA = 500;

    // Used by the DistanceController
    public const int I_GAPDISTANCESPEED = 500;

    // Used by WindManager
    public const float F_ANGLEPENNANTDEFAULT = 90;
    
}