using System.Collections;
using UnityEngine;

public class ScyllaTentacles : MonoBehaviour
{
    private float f_RotationSpeed = 75;

    public void UpdateTentacle()
    {
        transform.Rotate(f_RotationSpeed * Time.deltaTime * Vector3.left);

        if (transform.rotation.eulerAngles.x < 200)
        {
            ResetTentactle();
        }
    }


    public void ResetTentactle()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Tentacle>().CanBeRemoved();
            }
        }

        StartCoroutine(ResetRotation());
    }

    private IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(1);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}