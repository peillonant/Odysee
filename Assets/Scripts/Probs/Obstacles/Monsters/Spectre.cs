using UnityEngine;

public class Sprectre : Obstacles
{
    Material objectMaterial;
    GameObject go_Ship;

    float f_speedSpectre = 20;

    // Variable linked to the Animation of the Spectre
    private float f_TimerAnime = 0;
    private readonly float f_DelayAnime = 1.5f;
    private readonly float topPos = 5f;

    void Start()
    {
        go_Ship = GameObject.Find("Ship");
        objectMaterial = GetComponentInChildren<Renderer>().material;
    }

    protected override void AttackShip()
    {
        // Compute the direction between the Ship and the Spectre to move toward it
        Vector3 v3_newPosition = Vector3.MoveTowards(transform.position, go_Ship.transform.position, f_speedSpectre * Time.deltaTime);
        v3_newPosition.y = transform.position.y;
        transform.position = v3_newPosition;

        // Add the animation of the Spectre to have the feeling it's floating
        this.transform.localPosition = GlobalAnimation.AnimationFloating(ref f_TimerAnime, f_DelayAnime, 0, topPos, this.transform.localPosition);
    }

    protected override void RemoveObstacle()
    {
        var newAlpha = Tweening.Lerp(ref f_TimerToRemove, f_DelayToRemove, 1, 0);

        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, newAlpha);

        if (f_TimerToRemove > f_DelayToRemove)
            ResetObstacle();
    }

    public override void ResetObstacle()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Monsters").transform);
        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, 1);
        b_CanBeRemove = false;
    }


    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched_SlowDown();

            // Impact the score of the player by removing 100points
            GameInfo.instance.DeacreaseScore(100);

            // When touched by the spectre, we increase the fog to loose the vision during 3 secondes [.75f seconds to modify the fog, 1.5f with the new fog and .75f to have the fog back to normal]
            go_Ship.GetComponent<VisionController>().SetFogChanging(2f);

            b_CanBeRemove = true;
        }
    }
}