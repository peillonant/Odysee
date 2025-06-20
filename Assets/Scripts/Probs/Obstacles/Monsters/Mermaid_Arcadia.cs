using UnityEngine;

public class Mermaid_Arcadia : Obstacles
{
    GameObject go_Ship;
    [SerializeField] new ParticleSystem particleSystem;

    float f_DistanceBeforePower = 500;
    float f_TimerPower = 0;
    float f_ColdDownPower = 3f;

    // Variable linked to the animation
    float f_TimerAnime = 0;
    float f_DelayAnime = 1.5f;
    float f_BottomPos = -2.5f;
    float f_TopPos = 2.5f;
    float f_DelayToSink = 1.5f;


    void Start()
    {
        go_Ship = GameObject.Find("Ship");
    }

    protected override void AttackShip()
    {
        // Compute the distance on Z between the Mermaid and the Ship
        float f_distanceBetweenBoth = transform.position.z - go_Ship.transform.position.z;

        // The mermaid can just attack when they in front of the ship
        if (f_distanceBetweenBoth > 0 && f_distanceBetweenBoth <= f_DistanceBeforePower)
        {
            f_TimerPower += Time.deltaTime;

            if (f_TimerPower >= f_ColdDownPower)
            {
                f_TimerPower = 0;

                particleSystem.Play();

                if (go_Ship.transform.position.x != transform.position.x)
                    go_Ship.GetComponent<ShipController>().TriggerAttraction_Attraction(transform.position.x);
            }
        }

        // Animation of the Mermaid
        this.transform.localPosition = GlobalAnimation.AnimationFloating(ref f_TimerAnime, f_DelayAnime, f_BottomPos, f_TopPos, this.transform.localPosition);
    }

    protected override void RemoveObstacle()
    {
        Vector3 newPosition = this.transform.localPosition;
        newPosition.y = Tweening.Lerp(ref f_TimerToRemove, f_DelayToSink, 0, -25);
        this.transform.localPosition = newPosition;

        if (f_TimerToRemove > f_DelayToRemove)
            ResetObstacle();
    }

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;
            GameInfo.instance.IncreaseScore(15);
        }
        else if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched();

            // Increase the noise volume by 3.
            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(6);

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;
        }
    }
}