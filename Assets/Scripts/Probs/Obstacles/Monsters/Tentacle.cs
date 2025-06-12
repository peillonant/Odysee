using UnityEngine;

public class Tentacle : Obstacles
{

    // Method to trigger the remove when the tentacle is bellow the sea
    public void CanBeRemoved() => b_CanBeRemove = true;

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;
        }

        base.OnTriggerEnter(other);
    }

    protected override void RemoveObstacle()
    {
        var newAlpha = Tweening.Lerp(ref f_TimerToRemove, f_DelayToRemove, 1, 0);

        if (f_TimerToRemove > f_DelayToRemove)
        {
            ResetObstacle();
        }
    }

    public override void ResetObstacle()
    {
        Destroy(gameObject);
    }
}