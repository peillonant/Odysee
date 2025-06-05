using UnityEngine;

public class Coin : Items
{
    float f_speedRotation = 100;
    bool b_BouncingTop = true;
    float f_BorderYMax = 5;
    float f_BorderYMin = 0;

    void Update()
    {
        AnimRotateCoin();
        AnimBouncingCoin();
    }

    void AnimRotateCoin()
    {
        transform.localRotation *= Quaternion.Euler(0, 0, f_speedRotation * Time.deltaTime);
    }

    void AnimBouncingCoin()
    {
        if (b_BouncingTop)
        {
            if (transform.localPosition.y >= f_BorderYMax)
                b_BouncingTop = false;
            else
                transform.localPosition += new Vector3(0, 7.5f, 0) * Time.deltaTime;
        }
        else
        {
            if (transform.localPosition.y <= f_BorderYMin)
                b_BouncingTop = true;
            else
                transform.localPosition -= new Vector3(0, 7.5f, 0) * Time.deltaTime;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            // We add the coin to the inventory of the player
            GameInfo.instance.IncreaseCoin(1);

            // We add the coin point on the score
            GameInfo.instance.IncreaseScore(1);

            // We inform the Oboles (Parent Object) that a coin has been collected
            transform.parent.GetComponent<Oboles>().CoinCollected();

            // We destroy the gameObject
            Destroy(gameObject);
        }
    }
}