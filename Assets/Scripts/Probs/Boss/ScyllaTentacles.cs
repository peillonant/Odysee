using System.Collections;
using UnityEngine;

public class ScyllaTentacles : MonoBehaviour
{
    private bool TriggerAttack = false;


    public void UpdateTentacle()
    {
        if (!TriggerAttack && transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponentInChildren<Animator>().SetTrigger("TriggerAttack");
            }
                
            TriggerAttack = true;
            StartCoroutine(ResetTentactleTimer());
        }
    }


    public void ResetTentactle()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponentInChildren<Tentacle>().CanBeRemoved();
            }
        }

        TriggerAttack = false;
    }

    private IEnumerator ResetTentactleTimer()
    {
        yield return new WaitForSeconds(2);
        ResetTentactle();
    }
}