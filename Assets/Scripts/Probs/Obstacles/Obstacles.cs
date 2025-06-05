using System;
using UnityEngine;

public enum TypeObstacles
{
    OBSTACLE,
    MONSTER
}

public enum TypeObstaclesSize
{
    SHORT,
    BIG
}


public abstract class Obstacles : MonoBehaviour
{
    [SerializeField] protected TypeObstacles typeObstacles;
    [SerializeField] protected TypeObstaclesSize typeObstaclesSize;

    protected bool b_CanBeRemove;
    protected float f_TimerToRemove = 0;
    protected float f_DelayToRemove = 0.25f;

    public TypeObstacles GetTypeObstacles() => typeObstacles;
    public TypeObstaclesSize GetTypeObstaclesSize() => typeObstaclesSize;

    void Update()
    {
        if (b_CanBeRemove)
            RemoveObstacle();

        if (!b_CanBeRemove)
            AttackShip();
    }

    // Check the collision with the Ship
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;
        }
    }
    
    protected virtual void AttackShip() {}

    protected abstract void RemoveObstacle();
    protected abstract void ResetObstacle();

}
