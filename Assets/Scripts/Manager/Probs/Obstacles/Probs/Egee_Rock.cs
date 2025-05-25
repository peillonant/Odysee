using UnityEngine;

public class Egee_Rock : Obstacles
{
    Material objectMaterial;

    void Start()
    {
        objectMaterial = GetComponent<Renderer>().material;
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
        gameObject.transform.SetParent(GameObject.Find("NotUsed/Obstacles").transform);
        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, 1);
        b_CanBeRemove = false;
    }
}