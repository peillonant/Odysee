using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scylla : Boss
{
    // Varialbe linked to the material of the Eyes and the color linked to the heath
    private Material eyesMaterial;
    private readonly Color fullHealth = new(0.6f, 1, 1);
    private readonly Color halfHealth = new(0.6f, 1, 0);
    private readonly Color lowHealth = new(0.6f, 0, 0);

    // Variable linked to the first Attack
    private int i_IndexLane_A1 = 0;
    private float f_TimerAttack_A1 = 0;
    private float f_DelayAttack_A1 = 2f;
    private readonly float f_ScyllaSpeed_Sideway = 50f;
    private readonly float f_ScyllaSpeed_Forward = 300f;
    private bool b_AttackedPerformed_A1 = false;
    private Vector3 v3_PositionTargetLane = new();
    private Vector3 v3_PositionTargetImpacted = new();

    // Variable linked to the Second Attack
    [SerializeField] GameObject go_PrefabTentacle;
    [SerializeField] GameObject go_TentaclesParent;
    private float f_TimerAttack_A2 = 0;
    private float f_DelayAttack_A2 = 2;
    private bool b_AttackedPerformed_A2 = false;
    private int i_NbTentacle = 0;

    // Variable linked to the Window where the boss can be attacked
    private float f_TimerCanBeTouched = 0;
    private readonly float f_DelayFirstStep = 0.5f;     // Going above the sea
    private readonly float f_DelaySecondStep = 2.5f;    // Stay above to be hit
    private readonly float f_DelayThirdStep = 3f;       // Going bellow the sea
    private readonly float f_PosBottom = -15;
    private readonly float f_PosTop = 20;

    // For Scylla, the color of the eye will be the indicator of the current health
    protected override void BossPrepareToFight()
    {
        if (eyesMaterial == null)
            eyesMaterial = this.transform.GetChild(0).GetComponent<MeshRenderer>().materials[2];

        if (i_CurrentHealth == i_BossHealth)
            eyesMaterial.color = fullHealth;
        else if (i_CurrentHealth == (int)i_BossHealth - (int)Mathf.Ceil(i_BossHealth / 2))
            eyesMaterial.color = halfHealth;
        else if (i_CurrentHealth == (int)Mathf.Ceil(i_BossHealth / 2))
            eyesMaterial.color = lowHealth;
    }

    protected override void FirstAttack()
    {
        // On the first call of the Attack, we decide on which lane it will attack (1 : Left and middle // 2 : Middle and Right)
        if (i_IndexLane_A1 == 0)
        {
            i_IndexLane_A1 = Random.Range(1, 3);

            if (i_IndexLane_A1 == 1)
                v3_PositionTargetLane = new(-GameConstante.I_BORDERX, transform.localPosition.y, transform.localPosition.z);
            else if (i_IndexLane_A1 == 2)
                v3_PositionTargetLane = new(GameConstante.I_BORDERX, transform.localPosition.y, transform.localPosition.z);

            v3_PositionTargetImpacted = v3_PositionTargetLane;
            v3_PositionTargetImpacted.z = -300;
        }

        f_TimerAttack_A1 += Time.deltaTime;

        // First we place the boss on the correct lane
        if (f_TimerAttack_A1 < f_DelayAttack_A1)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, v3_PositionTargetLane, f_ScyllaSpeed_Sideway * Time.deltaTime);
        else
        {
            // Now we are moving Forward (or Backward when attacked performed)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, v3_PositionTargetImpacted, f_ScyllaSpeed_Forward * Time.deltaTime);

            // Check if the position of Scylla was at the Impact area. If so, the new target is to be back to the inital place
            if (transform.localPosition.z <= v3_PositionTargetImpacted.z && !b_AttackedPerformed_A1)
            {
                v3_PositionTargetImpacted.x = 0;
                v3_PositionTargetImpacted.z = 0;
                b_AttackedPerformed_A1 = true;
            }

            // After the reposition, we end the Attack and we reset all variable linked to the Attack
            if (transform.localPosition.z == 0 && b_AttackedPerformed_A1)
            {
                i_IndexLane_A1 = 0;
                f_TimerAttack_A1 = 0;
                b_AttackedPerformed_A1 = false;
                queueAttack.Dequeue();
            }
        }
    }

    protected override void SecondAttack()
    {
        i_NbTentacle = go_TentaclesParent.transform.childCount;

        // We generate the number of Tentacle to instantiate
        if (i_NbTentacle == 0 && !b_AttackedPerformed_A2)
        {
            b_AttackedPerformed_A2 = true;
            CreateTentacle();
        }

        f_TimerAttack_A2 += Time.deltaTime;

        if (f_TimerAttack_A2 > f_DelayAttack_A2)
        {
            // Now we can send Tentacle Prefab to the ship
            if (i_NbTentacle > 0)
            {
                go_TentaclesParent.GetComponent<ScyllaTentacles>().UpdateTentacle();
            }
            else
            {
                f_TimerAttack_A2 = 0;
                b_AttackedPerformed_A2 = false;
                go_TentaclesParent.GetComponent<ScyllaTentacles>().ResetTentactle();
                queueAttack.Dequeue();
            }
        }
    }

    private void CreateTentacle()
    {
        i_NbTentacle = Random.Range(1, 2 + i_NbDefeated);
        i_NbTentacle = i_NbTentacle > 3 ? 3 : i_NbTentacle;

        for (int i = 0; i < i_NbTentacle; i++)
        {
            GameObject go_newTentacle = Instantiate(go_PrefabTentacle, this.transform.GetChild(1));
            Vector3 v3_NewPositionTentacle = go_newTentacle.transform.localPosition;

            if (i == 0)
            {
                // We have to select a lane (0 - Left, 1 - Middle, 2 - Right)
                int i_IndexLane = Random.Range(0, 3);

                switch (i_IndexLane)
                {
                    case 0:
                        v3_NewPositionTentacle.x = ((float)-GameConstante.I_BORDERX);
                        break;
                    case 1:
                        v3_NewPositionTentacle.x = 0;
                        break;
                    case 2:
                        v3_NewPositionTentacle.x = ((float)GameConstante.I_BORDERX);
                        break;
                }
            }
            else if (i == 1)
            {
                // We have to select a lane (0 - Left, 1 - Middle) if the lane already taken then we put it on the Right
                int i_IndexLane = Random.Range(0, 2);

                if (i_IndexLane == 0)
                {
                    if (transform.GetChild(1).GetChild(0).localPosition.x == -GameConstante.I_BORDERX)
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX;
                    else
                        v3_NewPositionTentacle.x = -GameConstante.I_BORDERX;
                }
                else if (i_IndexLane == 1)
                {
                    if (transform.GetChild(1).GetChild(0).localPosition.x == 0)
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX;
                    else
                        v3_NewPositionTentacle.x = 0;
                }
            }
            else if (i == 2)
            {
                List<int> i_IndexLane = new List<int> { 0, 1, 2 };

                for (int j = 0; j < i; j++)
                {
                    Transform tentacleCreated = transform.GetChild(1).GetChild(j);

                    if (tentacleCreated.localPosition.x == -GameConstante.I_BORDERX)
                        i_IndexLane.Remove(0);
                    else if (tentacleCreated.localPosition.x == 0)
                        i_IndexLane.Remove(1);
                    else if (tentacleCreated.localPosition.x == GameConstante.I_BORDERX)
                        i_IndexLane.Remove(2);
                }

                switch (i_IndexLane[0])
                {
                    case 0:
                        v3_NewPositionTentacle.x = -GameConstante.I_BORDERX;
                        break;
                    case 1:
                        v3_NewPositionTentacle.x = 0;
                        break;
                    case 2:
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX;
                        break;
                }
            }

            go_newTentacle.transform.localPosition = v3_NewPositionTentacle;
        }
    }

    protected override void FrameVulnerable()
    {
        b_Canbetouched = true;

        f_TimerCanBeTouched += Time.deltaTime;

        Vector3 newPosition = this.transform.localPosition;

        // Moving Scylla to indicate to the player that it can be hit
        if (f_TimerCanBeTouched < f_DelayFirstStep)                                                             // Going to the top
            newPosition.y = Mathf.Lerp(f_PosBottom, f_PosTop, f_TimerCanBeTouched / f_DelayFirstStep);
        else if (f_TimerCanBeTouched > f_DelaySecondStep && f_TimerCanBeTouched < f_DelayThirdStep)             // Going to the bottom
            newPosition.y = Mathf.Lerp(f_PosTop, f_PosBottom, f_TimerCanBeTouched / f_DelayThirdStep);

        this.transform.localPosition = newPosition;

        if (f_TimerCanBeTouched > f_DelayThirdStep)
        {
            ResetFrameVulnerable();
        }
    }

    protected override void ResetFrameVulnerable()
    {
        b_Canbetouched = false;
        f_TimerAttack = 0;
        f_TimerCanBeTouched = 0;
        queueAttack.Dequeue();
    }

    // Method called when Scylla has been hit by the bullet
    protected override void TriggerHasBeenHit()
    {
        f_TimerBeenTouched += Time.deltaTime;

        // First we check if we need to change the color of the eye regarding the current life
        if (i_CurrentHealth > (i_BossHealth / 2) && eyesMaterial.color != fullHealth)
            eyesMaterial.color = fullHealth;
        else if (i_CurrentHealth > (i_BossHealth / 3) && eyesMaterial.color != halfHealth)
            eyesMaterial.color = Color.Lerp(fullHealth, halfHealth, f_TimerBeenTouched / f_DelayBeenTouched);
        else if (i_CurrentHealth <= (i_BossHealth / 3) && eyesMaterial.color != lowHealth)
            eyesMaterial.color = Color.Lerp(halfHealth, lowHealth, f_TimerBeenTouched / f_DelayBeenTouched);

        // Second, we put back Scylla bellow to the sea
        this.transform.localPosition = Vector3.Lerp(v3_PositionHit, v3_defaultLocalPosition, f_TimerBeenTouched / f_DelayBeenTouched);

        if (f_TimerBeenTouched > f_DelayBeenTouched)
        {
            f_TimerBeenTouched = 0;
            b_HasBeenTouched = false;
            ResetFrameVulnerable();
        }
    }

    // Methods linked to the defait of the boss
    protected override void RemoveBoss()
    {
        f_TimerToRemove += Time.deltaTime;

        this.transform.localPosition = Vector3.Lerp(v3_defaultLocalPosition, new Vector3(v3_defaultLocalPosition.x, -55, v3_defaultLocalPosition.z), f_TimerToRemove / f_DelayToRemove);

        if (f_TimerToRemove > f_DelayToRemove)
        {
            f_TimerToRemove = 0;
            bossDefeatEvent.Invoke();
            ResetBoss();
        }
    }
}