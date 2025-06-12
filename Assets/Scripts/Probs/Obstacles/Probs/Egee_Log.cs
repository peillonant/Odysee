using System.Collections.Generic;
using UnityEngine;

public class Egee_Log : Obstacles
{
    [SerializeField] List<Mesh> meshLogs;

    public void OnEnable()
    {
        int i_indexMesh = Random.Range(0, 2);
        GetComponent<MeshFilter>().sharedMesh = meshLogs[i_indexMesh];
        transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = meshLogs[i_indexMesh];
    }


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

        // Deactivate the Renderer that contain the material opaque to display the renderer with the tranparent settings
        if (GetComponent<MeshRenderer>().enabled)
            GetComponent<MeshRenderer>().enabled = false;

        // Now we are playing with the transparancy
        Material materialLog = transform.GetChild(0).GetComponent<Renderer>().material;
        materialLog.color = new Color(materialLog.color.r, materialLog.color.g, materialLog.color.b, newAlpha);

        if (f_TimerToRemove > f_DelayToRemove)
            ResetObstacle();
    }

    public override void ResetObstacle()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Obstacles").transform);

        // Put back the renderer of the main object
        GetComponent<MeshRenderer>().enabled = true;

        // Then put back the transparancy to 1
        Material materialLog = transform.GetChild(0).GetComponent<Renderer>().material;
        materialLog.color = new Color(materialLog.color.r, materialLog.color.g, materialLog.color.b, 1);

        b_CanBeRemove = false;
    }
}