using UnityEngine;

public class Jellyfish : Obstacles
{
    Material objectMaterial;
    GameObject go_Ship;

    float f_speedJellyfish = 40;

    void Start()
    {
        go_Ship = GameObject.Find("Ship");
        objectMaterial = GetComponent<Renderer>().material;
    }

    protected override void AttackShip()
    {
        // Compute the direction between the Ship and the JellyFish to move toward it
        Vector3 v3_newPosition = Vector3.MoveTowards(transform.position, go_Ship.transform.position, f_speedJellyfish * Time.deltaTime);
        v3_newPosition.y = transform.position.y;
        transform.position = v3_newPosition;
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
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/Monsters").transform);
        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, 1);
        b_CanBeRemove = false;
    }


    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;
        }
        else if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched_JellyFish();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;
        }
    }
}