using System.Collections.Generic;
using UnityEngine;

public class Arcadia_Log : Obstacles
{
    float f_DelayToRemoveLog = 2;

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            b_CanBeRemove = true;

            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(4);
        }

        if (other.CompareTag("Ship"))
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();

            b_CanBeRemove = true;

            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(2);
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