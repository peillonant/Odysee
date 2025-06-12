using Unity.Collections;
using UnityEngine;

public class SpiritHunter : Obstacles
{
    GameObject go_Ship;

    // Variable of the position by default
    private Vector3 v3_DefaultPosition = new Vector3(0, -30, 0);


    // Variable linked to the Animation of the Hand
    private bool b_IsCloseEnough = false;
    private float f_DistanceToAppear = 200;
    private float f_DistanceToAttack = 75;
    private float f_SpeedHandToAppear = 50;

    private float f_TimerAnime = 0;
    private readonly float f_DelayAnime = 1.5f;
    

    // Variable linked to the Removing Action
    private float f_TargetAngle = 0;
    private Vector3 v3_RotationBeforeFalling;
    private float f_DelayRemove = 1;

    void Start()
    {
        go_Ship = GameObject.Find("Ship");
        this.transform.localPosition += v3_DefaultPosition;
    }

    void OnEnable()
    {
        b_IsCloseEnough = false;
    }

    protected override void AttackShip()
    {
        // Compute the distance between the Ship and the hand. If bellow the Gap, the Hand appear from the sea
        if (!b_IsCloseEnough && go_Ship.transform.position.z > this.transform.position.z - f_DistanceToAppear)
            b_IsCloseEnough = true;

        else if (b_IsCloseEnough && this.transform.localPosition.y != 0)
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z), f_SpeedHandToAppear * Time.deltaTime);

        // When the ship is close enough, the Hand try to go down on it
        if (go_Ship.transform.position.z > this.transform.position.z - f_DistanceToAttack) { }
        //AnimationSpectre();
    }

    private void AnimationSpectre()
    {
        f_TimerAnime += Time.deltaTime;

        Vector3 newRotation = Vector3.zero;
        newRotation.x = Mathf.Lerp(0, -135, f_TimerAnime / f_DelayAnime);
        this.transform.localEulerAngles = newRotation;

        if (f_TimerAnime > f_DelayAnime)
        {
            f_TimerAnime = 0;
            RemoveObstacle();
        }
    }

    protected override void RemoveObstacle()
    {
        f_TimerToRemove += Time.deltaTime;

        Vector3 newRotation = this.transform.localRotation.eulerAngles;
        newRotation.x = Mathf.Lerp(v3_RotationBeforeFalling.x, f_TargetAngle, f_TimerToRemove / f_DelayRemove);
        this.transform.localRotation = Quaternion.Euler(newRotation);

        if (f_TimerToRemove > f_DelayRemove)
        {
            f_TimerToRemove = 0;
            ResetObstacle();
        }
    }

    public override void ResetObstacle()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Monsters").transform);
        b_CanBeRemove = false;
        b_IsCloseEnough = false;

        // Reset Position and localRotation
        this.transform.SetLocalPositionAndRotation(v3_DefaultPosition, Quaternion.Euler(Vector3.zero));

        this.GetComponent<BoxCollider>().enabled = true;
    }


    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
            GameInfo.instance.IncreaseScore(5);

        if (other.CompareTag("Ship"))
            {
                // Reduce the speed
                other.GetComponent<ShipController>().HasBeenTouched();

                // Trigger the InvuFrame of the Ship
                other.GetComponent<ColliderController>().TriggerInvuFrame();

                // Trigger the method to increase the noise that will trigger the boss
                GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(3);

                b_CanBeRemove = true;
                f_TargetAngle = 90;
                v3_RotationBeforeFalling = this.transform.localEulerAngles;
            }

        if (other.CompareTag("Bullet") || other.CompareTag("Ship"))
        {
            b_CanBeRemove = true;
            f_TargetAngle = 90;
            v3_RotationBeforeFalling = this.transform.localEulerAngles;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}