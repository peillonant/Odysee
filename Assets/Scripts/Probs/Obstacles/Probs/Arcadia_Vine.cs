using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcadia_Vine : Obstacles
{
    float f_GapBeforeMoving = 500;
    float f_DelayToRemoveLog = 2;
    float f_TimerMovementVine = 0;
    float f_DelayMovementVine = 5;
    float f_TargetPositionX = 0;
    Vector3 v3_OriginalPosition;
    GameObject go_ship;

    void Start()
    {
        go_ship = GameObject.Find("Ship");   
    }

    void OnEnable()
    {
        // StartCoroutine to let the time to be put in the correct place then we launch the position of the Vine
        StartCoroutine(DirectionVine());  
    }

    // Coroutine that will compute the direction of Vine correctly regarding the position of it during the creation of the sea
    private IEnumerator DirectionVine()
    {
        yield return new WaitForSeconds(.5f);
        v3_OriginalPosition = this.transform.localPosition;

        if (this.transform.localPosition.x == -GameConstante.I_BORDERX)
        {
            f_TargetPositionX = GameConstante.I_BORDERX;
        }
        else if (this.transform.localPosition.x == GameConstante.I_BORDERX)
        {
            f_TargetPositionX = -GameConstante.I_BORDERX;
        }
        else
        {
            int rng = UnityEngine.Random.Range(0, 1);
            if (rng == 0)
            {
                f_TargetPositionX = GameConstante.I_BORDERX;
            }
            else
            {
                f_TargetPositionX = -GameConstante.I_BORDERX;
            }
        }
    }

    // Method that manage the movement of the Vine going to the TargetPosition
    protected override void AttackShip()
    {
        if (go_ship.transform.position.z > this.transform.position.z - f_GapBeforeMoving)
        {
            f_TimerMovementVine += Time.deltaTime;

            Vector3 newPosition = this.transform.localPosition;
            newPosition.x = Mathf.Lerp(v3_OriginalPosition.x, f_TargetPositionX, f_TimerMovementVine / f_DelayMovementVine);

            if (f_TimerMovementVine > f_DelayMovementVine)
            {
                f_TimerMovementVine = 0;
                b_CanBeRemove = true;
            }
        }
       
    }


    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;

            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseAnger(4);
        }

        if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;

            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseAnger(2);
        }
    }


    protected override void RemoveObstacle()
    {
        // Deactivate the Renderer that contain the material opaque to display the renderer with the tranparent settings
        if (GetComponentInChildren<MeshRenderer>().enabled)
            GetComponentInChildren<MeshRenderer>().enabled = false;

        f_TimerToRemove += Time.deltaTime;
        Vector3 newPosition = this.transform.localPosition;
        newPosition.y = Mathf.Lerp(0, -20, f_TimerToRemove / f_DelayToRemoveLog);
        this.transform.localPosition = newPosition;

        if (f_TimerToRemove > f_DelayToRemoveLog)
        {
            f_TimerToRemove = 0;
            ResetObstacle();
        }
    }

    public override void ResetObstacle()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Obstacles").transform);

        // Put back the renderer of the main object
        GetComponentInChildren<MeshRenderer>().enabled = true;

        this.transform.localPosition = Vector3.zero;

        b_CanBeRemove = false;
    }
}