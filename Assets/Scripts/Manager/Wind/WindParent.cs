using UnityEngine;


// Script that allow the Boss Parent GameObject to follow the position in z of the Ship when the Boss fight is on
public class WindParent : MonoBehaviour
{
    [SerializeField] GameObject go_Ship;

    float f_GapWithShip = 25;

    void FixedUpdate()
    {
        FollowShip();
    }

    private void FollowShip()
    {
        var newPosition = transform.position;
        newPosition.z = go_Ship.transform.position.z + f_GapWithShip;
        transform.position = newPosition;
    }
}