using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    // Varialbe linked to the Boss
    protected int i_BossHealth = 3;
    protected int i_CurrentHealth = 3;
    protected int i_NbDefeated = 0;
    protected bool b_BossDefeat = false;
    protected bool b_IsPreFight = false;
    protected Vector3 v3_defaultLocalPosition;
    public UnityEvent bossDefeatEvent;

    // Variable linked to Boss that can be touched
    protected bool b_Canbetouched = false;
    protected bool b_HasBeenTouched = false;
    protected float f_TimerBeenTouched = 0;
    protected float f_DelayBeenTouched = 2;
    protected Vector3 v3_PositionHit;

    // Variable linked to the Remove method
    protected float f_TimerToRemove = 0;
    protected float f_DelayToRemove = 1.5f;

    // Variable linked to the fight ability
    protected float f_TimerAttack = 0;
    protected float f_CoolDownAttack = 2;
    protected Queue<Action> queueAttack = new();

    // Encapsulation (Use for the UI)
    public int GetCurrentHealth() => i_CurrentHealth;

    void Start()
    {
        v3_defaultLocalPosition = this.transform.localPosition;
        bossDefeatEvent.AddListener(GameObject.Find("Boss").GetComponent<BossManager>().TriggerReward);
    }

    // Method called by the BossManager when the boss has been trigger
    public void SetIsPreFight(bool newState)
    {
        if (newState)                               // If it's a prefight boss we put the current life to MaxLife / 2 
            i_CurrentHealth = (int)Mathf.Ceil(i_BossHealth / 2);
        else if (b_IsPreFight && !newState)         // if the boss has been already killed on the pre-fight and we are now on the EndRegion fight
            i_CurrentHealth = (int)i_BossHealth - (int)Mathf.Ceil(i_BossHealth / 2);
        else if (!b_IsPreFight && !newState)        // If the boss trigger on the EndRegion fight and we did not have the prefight boss
            i_CurrentHealth = i_BossHealth;

        b_IsPreFight = newState;

        BossPrepareToFight();
    }

    // Method that will allow all boss to have their method to be prepare before the fight (change material or position)
    protected virtual void BossPrepareToFight() { }

    protected virtual void Update()
    {
        if (!GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            if (i_CurrentHealth <= 0)
            {
                // We killed the boss
                b_BossDefeat = true;

                // Removing the Boss
                RemoveBoss();

                // Put back the game to normal behavior
                GameInfo.instance.SetBossFight(false);
            }
            else if (b_HasBeenTouched)
            {
                TriggerHasBeenHit();
            }
            else
            {
                if (queueAttack.Count == 0)
                    GenerationQueueAttack();

                CheckAttackBoss();
            }
        }
    }

    // Methods linked to the defait of the boss
    protected virtual void RemoveBoss() { }
    protected virtual void ResetBoss()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Boss").transform);
        this.transform.localPosition = v3_defaultLocalPosition;
        i_CurrentHealth = 1;
        b_BossDefeat = false;

        // If when we reset the boss, it was not the prefight, that means it was the end region boss, so we can increase the max Health for the next time
        if (!b_IsPreFight)
        {
            i_BossHealth++;
            i_NbDefeated++;
        }
    }

    #region Attack phase of boss
    // Method that will generate all attacks of the Boss
    protected virtual void GenerationQueueAttack()
    {
        int i_NbAttack = 2 + i_NbDefeated;

        // On the generation Queue, we will always have the first Attack on the pattern. Then the rest of attack available will be chose randomly between both attack
        queueAttack.Enqueue(FirstAttack);

        for (int i = 1; i < i_NbAttack; i++)
        {
            //int indexAttack = Random.Range(0, 2);
            int indexAttack = 1;

            if (indexAttack == 0)
                queueAttack.Enqueue(FirstAttack);
            else
                queueAttack.Enqueue(SecondAttack);
        }

        // Then we add the Vulnerable Frame that allow the player to attack the boss
        queueAttack.Enqueue(FrameVulnerable);
    }

    protected virtual void CheckAttackBoss()
    {
        f_TimerAttack += Time.deltaTime;

        if (f_TimerAttack > f_CoolDownAttack)
        {
            queueAttack.Peek().Invoke();
        }
    }

    protected virtual void FirstAttack() { }
    protected virtual void SecondAttack() { }
    #endregion

    #region Phase Vulnerable
    protected virtual void FrameVulnerable() { }

    protected virtual void ResetFrameVulnerable() { }

    protected virtual void TriggerHasBeenHit() { }
    #endregion

    #region Collider
    // Check the collision with the Ship
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && !b_BossDefeat)
        {
            // Reduce the speed
            other.GetComponent<ShipController>().HasBeenTouched();

            // Trigger the InvuFrame of the Ship
            other.GetComponent<ColliderController>().TriggerInvuFrame();
        }

        if (other.CompareTag("Bullet") && b_Canbetouched)
        {
            i_CurrentHealth--;

            v3_PositionHit = this.transform.localPosition;

            // Need to trigger an animation of boss touched
            b_HasBeenTouched = true;
        }
    }
    #endregion

}