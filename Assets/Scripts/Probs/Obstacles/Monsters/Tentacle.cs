using UnityEngine;

public class Tentacle : Obstacles
{
    Material objectMaterial;

    void Start()
    {
        objectMaterial = GetComponent<Renderer>().material;
    }

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

        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, newAlpha);

        if (f_TimerToRemove > f_DelayToRemove)
        {
            ResetObstacle();
        }
    }

    protected override void ResetObstacle()
    {
        Destroy(gameObject);
    }
}