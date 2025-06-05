using UnityEngine;

public class CannonBall : MonoBehaviour
{
    float f_TimerLife = 0;
    float f_LifeTime = 1.5f;
    float f_Speed;

    void Start()
    {
        f_Speed = GameInfo.instance.GetCurrentSpeed() * 2;
    }

    // Update is called once per frame
    void Update()
    {
        f_TimerLife += Time.deltaTime;

        if (f_TimerLife < f_LifeTime)
        {
            MovingForward();
        }
    }

    private void MovingForward()
    {
        Vector3 v3_newPosition = transform.position;
        v3_newPosition.z += f_Speed * Time.deltaTime * 10;

        transform.position = v3_newPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin") || other.CompareTag("Ammo"))
            return;

        Destroy(gameObject);
    }

}
