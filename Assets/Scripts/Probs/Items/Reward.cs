using System.Collections.Generic;
using UnityEngine;

public class Reward : Items
{
    bool b_BouncingTop = true;
    float f_BorderYMax = 6;
    float f_BorderYMin = 0;

    void Start()
    {
        this.GetComponent<Renderer>().material = GameInfo.instance.GetCurrentRegion().rewardMaterial;
    } 

    void Update()
    {
        AnimBouncingReward();
    }

    void AnimBouncingReward()
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
            GameInfo.instance.AddRewardToList();

            GameInfo.instance.IncreaseScore(500);

            base.DestroyIt();
        }
    }
}