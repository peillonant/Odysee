using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scylla : Boss
{
    // Variable linked to the first Attack
    private int i_IndexLane_A1 = 0;
    private float f_TimerAttack_A1 = 0;
    private float f_DelayAttack_A1 = 2f;
    private float f_ScyllaSpeed_Sideway = 50f;
    private float f_ScyllaSpeed_Forward = 300f;
    private bool b_AttackedPerformed_A1 = false;
    private Vector3 v3_PositionTargetLane = new();
    private Vector3 v3_PositionTargetImpacted = new();

    // Variable linked to the Second Attack
    [SerializeField] GameObject go_PrefabTentacle;
    private float f_TimerAttack_A2 = 0;
    private float f_DelayAttack_A2 = 2;
    private bool b_AttackedPerformed_A2 = false;
    private int i_NbTentacle = 0;


    // Variable linked to the Window where the boss can be attacked
    private bool b_InvuCalled = false;
    private float f_TimerInvuColor = 0;
    private float f_DelayFirstStep = 0.5f;
    private float f_DelaySecondStep = 2.5f;

    // Varialbe linked to the frame when the boss has been hit
    private float f_ColorGNormal = 0.5f;
    private float f_ColorGHit = 0.25f;
    private Color DefaultColor = new Color(1, 0.5f, 0.4f);

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
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, v3_PositionTargetImpacted, f_ScyllaSpeed_Forward * Time.deltaTime);

            if (transform.localPosition.z <= v3_PositionTargetImpacted.z && !b_AttackedPerformed_A1)
            {
                v3_PositionTargetImpacted.x = 0;
                v3_PositionTargetImpacted.z = 0;
                b_AttackedPerformed_A1 = true;
            }

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
        i_NbTentacle = transform.GetChild(0).childCount;

        // We generate the number of Tentacle to instantiate
        if (i_NbTentacle == 0 && !b_AttackedPerformed_A2)
        {
            CreateTentacle();
        }

        f_TimerAttack_A2 += Time.deltaTime;

        if (f_TimerAttack_A2 > f_DelayAttack_A2)
        {
            // Now we can send Tentacle Prefab to the ship
            if (i_NbTentacle > 0)
            {
                if (!b_AttackedPerformed_A2) b_AttackedPerformed_A2 = true;
                transform.GetChild(0).GetComponent<ScyllaTentacles>().UpdateTentacle();
            }
            else
            {
                f_TimerAttack_A2 = 0;
                b_AttackedPerformed_A2 = false;
                transform.GetChild(0).GetComponent<ScyllaTentacles>().ResetTentactle();
                queueAttack.Dequeue();
            }
        }
    }

    protected override void FrameVulnerable()
    {
        b_Canbetouched = true;

        var MaterialColor = objectMaterial.color;

        f_TimerInvuColor += Time.deltaTime;

        // Going from Orange to Green
        if (f_TimerInvuColor < f_DelayFirstStep)
            MaterialColor.r = Mathf.Lerp(1, 0, f_TimerInvuColor / f_DelayFirstStep);
        else if (f_TimerInvuColor > f_DelaySecondStep)
            MaterialColor.r = Mathf.Lerp(0, 1, f_TimerInvuColor / f_DelaySecondStep);

        objectMaterial.color = MaterialColor;

        // we start a courotine, to let the player a window of few seconds to hit the boss
        if (!b_InvuCalled)
        {
            b_InvuCalled = true;
            StartCoroutine(WindowVulnerable());
        }
    }

    protected override IEnumerator WindowVulnerable()
    {
        yield return new WaitForSeconds(3);

        // Reset variable linked to the Vulnerable frame
        objectMaterial.color = new(1, objectMaterial.color.g, objectMaterial.color.b);
        b_InvuCalled = false;
        f_TimerInvuColor = 0;

        // Reset variable linked to Attack
        b_Canbetouched = false;
        f_TimerAttack = 0;
        queueAttack.Dequeue();
    }

    // Method linked to the blink of the boss when hit
    protected override void ResetColor()
    {
        objectMaterial.color = DefaultColor;
    }

    protected override void TriggerBlinkBoss()
    {
        f_TimerBeenTouched += Time.deltaTime;

        var newColorMaterial = objectMaterial.color;
        if (f_TimerBeenTouched < f_DelayInvu / 2)
        {
            newColorMaterial.g = Mathf.Lerp(f_ColorGNormal, f_ColorGHit, f_TimerBeenTouched / f_DelayInvu);
        }
        else
        {
            newColorMaterial.g = Mathf.Lerp(f_ColorGHit, f_ColorGNormal, f_TimerBeenTouched / f_DelayInvu);
        }

        objectMaterial.color = newColorMaterial;


        if (f_TimerBeenTouched > f_DelayBeenTouched)
        {
            f_TimerBeenTouched = 0;
            b_HasBeenTouched = false;
        }
    }
    
    private void CreateTentacle()
    {
        i_NbTentacle = Random.Range(1, 2 + i_NbDefeated);
        i_NbTentacle = i_NbTentacle > 3 ? 3 : i_NbTentacle;

        for (int i = 0; i < i_NbTentacle; i++)
        {
            GameObject go_newTentacle = Instantiate(go_PrefabTentacle, this.transform.GetChild(0));
            Vector3 v3_NewPositionTentacle = go_newTentacle.transform.localPosition;

            if (i == 0)
            {
                // We have to select a lane (0 - Left, 1 - Middle, 2 - Right)
                int i_IndexLane = Random.Range(0, 3);

                switch (i_IndexLane)
                {
                    case 0:
                        v3_NewPositionTentacle.x = ((float)-GameConstante.I_BORDERX) / 100;
                        break;
                    case 1:
                        v3_NewPositionTentacle.x = 0;
                        break;
                    case 2:
                        v3_NewPositionTentacle.x = ((float)GameConstante.I_BORDERX) / 100;
                        break;
                }
            }
            else if (i == 1)
            {
                // We have to select a lane (0 - Left, 1 - Middle) if the lane already taken then we put it on the Right
                int i_IndexLane = Random.Range(0, 2);

                if (i_IndexLane == 0)
                {
                    if (transform.GetChild(0).GetChild(0).localPosition.x == -GameConstante.I_BORDERX / 100)
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX / 100;
                    else
                        v3_NewPositionTentacle.x = -GameConstante.I_BORDERX / 100;
                }
                else if (i_IndexLane == 1)
                {
                    if (transform.GetChild(0).GetChild(0).localPosition.x == 0)
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX / 100;
                    else
                        v3_NewPositionTentacle.x = 0;
                }
            }
            else if (i == 2)
            {
                List<int> i_IndexLane = new List<int> { 0, 1, 2 };

                for (int j = 0; j < i; j++)
                {
                    Transform tentacleCreated = transform.GetChild(0).GetChild(j);

                    if (tentacleCreated.localPosition.x == -GameConstante.I_BORDERX / 100)
                        i_IndexLane.Remove(0);
                    else if (tentacleCreated.localPosition.x == 0)
                        i_IndexLane.Remove(1);
                    else if (tentacleCreated.localPosition.x == GameConstante.I_BORDERX / 100)
                        i_IndexLane.Remove(2);
                }

                switch (i_IndexLane[0])
                {
                    case 0:
                        v3_NewPositionTentacle.x = -GameConstante.I_BORDERX / 100;
                        break;
                    case 1:
                        v3_NewPositionTentacle.x = 0;
                        break;
                    case 2:
                        v3_NewPositionTentacle.x = GameConstante.I_BORDERX / 100;
                        break;
                }
            }

            go_newTentacle.transform.localPosition = v3_NewPositionTentacle;
        }
    }

    
}