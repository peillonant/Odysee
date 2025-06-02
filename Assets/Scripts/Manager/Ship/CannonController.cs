using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject go_CannonBallPrefab;
    [SerializeField] GameObject go_Cannon;

    // Bullet force
    private float shootForce, upwardForce;

    private bool b_OnCD;
    private float f_TimerCD = 0;
    private float f_DelayBeforeShoot = 2f;

    void Update()
    {
        if (!GameInfo.IsGameOnPause() && b_OnCD)
        {
            f_TimerCD += Time.deltaTime; 
        }
    }

    // Manage the Cannon
    public void TriggerCanon()
    {
        // First things to check is: we have at least one Ammo
        // Second things to check is the colddown of the Canon (to avoid using a machine gun)
        if (GameInfo.GetNbAmmo() > 0 && CheckCDCannon() && !GameInfo.IsGameOnPause() && !GameInfo.IsGameLost())
        {
            b_OnCD = true;
            ShootCannonBall();
        }
    }

    // Method that check the Cannon CD
    private bool CheckCDCannon()
    {
        if (b_OnCD && f_TimerCD > f_DelayBeforeShoot)
        {
            f_TimerCD = 0;
            b_OnCD = false;
            return true;
        }
        else if (b_OnCD && f_TimerCD < f_DelayBeforeShoot)
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
        
        GameInfo.DescreaseAmmo();
    }
}