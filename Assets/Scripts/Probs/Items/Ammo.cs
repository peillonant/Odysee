using UnityEngine;

public class Ammo : Items
{
    bool b_BouncingTop = true;
    float f_BorderYMax = 5;
    float f_BorderYMin = 0;

    void Update()
    {
        AnimBouncingCoin();
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
            GameInfo.instance.IncreaseAmmo();
            base.DestroyIt();
        }
    }
}