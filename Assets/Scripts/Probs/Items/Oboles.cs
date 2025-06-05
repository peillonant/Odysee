using UnityEngine;

public class Oboles : Items
{
    int i_NbCoinCollected = 0;

    public override void DestroyIt()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Items>().DestroyIt();
            }

            Destroy(gameObject);
        }
    }

    public void CoinCollected()
    {
        i_NbCoinCollected++;

        if (i_NbCoinCollected == 10)
        {
            // We collect all the oboles, so we increase the score by 9 (the last coin already increase the score by 1)
            GameInfo.instance.IncreaseCoin(9);

            DestroyIt();
        }
    }
}