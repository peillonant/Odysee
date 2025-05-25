using UnityEngine;

public class Items : MonoBehaviour
{
    public virtual void DestroyIt()
    {
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other) {}
}