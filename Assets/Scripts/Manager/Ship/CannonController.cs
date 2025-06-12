using UnityEngine;
using UnityEngine.Events;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject go_CannonBallPrefab;
    [SerializeField] GameObject go_Cannon;

    public UnityEvent CannonTrigger;

    private bool b_OnCD;
    private float f_TimerCD = 0;

    void Update()
    {
        if (!GameInfo.instance.IsGameOnPause() && b_OnCD)
        {
            f_TimerCD += Time.deltaTime; 
        }
    }

    // Manage the Cannon
    public void TriggerCanon()
    {
        // First things to check is: we have at least one Ammo
        // Second things to check is the colddown of the Canon (to avoid using a machine gun)
        if (GameInfo.instance.GetNbAmmo() > 0 && CheckCDCannon() && !GameInfo.instance.IsGameOnPause() && !GameInfo.instance.IsGameLost())
        {
            b_OnCD = true;
            ShootCannonBall();
        }
    }

    // Method that check the Cannon CD
    private bool CheckCDCannon()
    {
        if (b_OnCD && f_TimerCD > GameConstante.F_CANNONCOLDDOWN)
        {
            f_TimerCD = 0;
            b_OnCD = false;
            return true;
        }
        else if (b_OnCD && f_TimerCD < GameConstante.F_CANNONCOLDDOWN)
        {
            return false;
        }
        else
            return true;

    }

    // Method that trigger the instanciation of the Cannonball and Add the Force on it
    private void ShootCannonBall()
    {
        Vector3 v3_CannonPosition = go_Cannon.transform.position;
        v3_CannonPosition.z += 15;

        Instantiate(go_CannonBallPrefab, v3_CannonPosition, Quaternion.identity, GameObject.Find("CannonBalls").transform);

        if (GameInfo.instance.TutorielHasBeenSeen())       // Allow the player to trigger the Cannon without losing Ammo
        {
            GameInfo.instance.DescreaseAmmo();
            //SoundManager.instance.PlaySound(SoundType.ACTION, 0, 0.5f);
        }

        CannonTrigger.Invoke();
    

        if (GameInfo.instance.GetCurrentRegion().typeRegion == TypeRegion.STYX)
        {
            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(1);
        }

    }
}