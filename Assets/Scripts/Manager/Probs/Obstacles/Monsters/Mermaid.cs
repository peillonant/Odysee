using UnityEngine;

public class Mermaid : Obstacles
{
    Material objectMaterial;
    GameObject go_Ship;

    float f_DistanceBeforePower = 500;
    float f_TimerPower = 0;
    float f_ColdDownPower = 3f;

    void Start()
    {
        go_Ship = GameObject.Find("Ship");
        objectMaterial = GetComponent<Renderer>().material;
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

                if (go_Ship.transform.position.x != transform.position.x)
                {
                    go_Ship.GetComponent<ShipController>().TriggerAttraction(transform.position.x);
                }
            }
        }
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
            other.GetComponent<ShipController>().HasBeenTouched();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;
        }
    }
}