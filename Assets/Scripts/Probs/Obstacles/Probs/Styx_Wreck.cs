using System.Collections.Generic;
using UnityEngine;

public class Styx_Wreck : Obstacles
{
    float f_DelayToSink = 1.5f;

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
            b_CanBeRemove = true;

        if (other.CompareTag("Ship"))
        {
            // Increase the noise volume by 3.
            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(3);
        }

        if (other.CompareTag("Bullet") || other.CompareTag("Ship"))
            this.GetComponent<BoxCollider>().enabled = false;

        base.OnTriggerEnter(other);
    }


    protected override void RemoveObstacle()
    {
        Vector3 newPosition = this.transform.localPosition;
        newPosition.y = Tweening.Lerp(ref f_TimerToRemove, f_DelayToSink, 0, -25);

        this.transform.localPosition = newPosition;

        if (f_TimerToRemove > f_DelayToSink)
            ResetObstacle();
    }
}