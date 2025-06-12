using UnityEngine;

public class Jellyfish : Obstacles
{
    GameObject go_Ship;

    float f_speedJellyfish = 40;
    float f_TimerAnime = 0;
    private readonly float f_DelayAnime = 1.5f;
    private readonly float topPos = 15f;
    float f_DelayToSink = 1.5f;

    void Start()
    {
        go_Ship = GameObject.Find("Ship");
    }

    protected override void AttackShip()
    {
        // Compute the direction between the Ship and the JellyFish to move toward it
        Vector3 v3_newPosition = Vector3.MoveTowards(transform.position, go_Ship.transform.position, f_speedJellyfish * Time.deltaTime);
        v3_newPosition.y = transform.position.y;
        transform.position = v3_newPosition;

        // Add the animation of the Jellyfish to have the feeling it's floating
        this.transform.localPosition = GlobalAnimation.AnimationFloating(ref f_TimerAnime, f_DelayAnime, 0, topPos, this.transform.localPosition);
    }

    protected override void RemoveObstacle()
    {
        Vector3 newPosition = this.transform.localPosition;
        newPosition.y = Tweening.Lerp(ref f_TimerToRemove, f_DelayToSink, 0, -25);

        this.transform.localPosition = newPosition;


        if (f_TimerToRemove > f_DelayToRemove)
        {
            ResetObstacle();
        }
    }

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;
            GameInfo.instance.IncreaseScore(5);
        }
        else if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched_SlowDown();

            // Trigger the InvuFrame of the Ship
            //other.GetComponent<ColliderController>().TriggerInvuFrame();

            // Impact the score of the player by removing 50points
            GameInfo.instance.DeacreaseScore(50);

            b_CanBeRemove = true;
        }
    }
}