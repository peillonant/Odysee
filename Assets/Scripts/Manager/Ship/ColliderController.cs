using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private List<GameObject> go_ShipImpactedByColor;

    bool b_InvuFrame;
    float f_TimerInvu;
    float f_DelayInvu = 2;
    float colorHasBeenHit = 0.4f;

    void Update()
    {
        // Update the timer of the invuFrame
        if (b_InvuFrame && !GameInfo.instance.IsGameOnPause())
        {
            f_TimerInvu += Time.deltaTime;

            TriggerBlinkShip(f_TimerInvu);

            if (f_TimerInvu > f_DelayInvu)
            {
                f_TimerInvu = 0;
                b_InvuFrame = false;
            }
        }
    }

    // Method that the color of the ship to red and back to normal when is been hit
    void TriggerBlinkShip(float f_Timer)
    {
        float newColorMaterial;
        if (f_Timer < f_DelayInvu / 2)
            newColorMaterial = Mathf.Lerp(1, colorHasBeenHit, f_Timer / f_DelayInvu);
        else
            newColorMaterial = Mathf.Lerp(colorHasBeenHit, 1, f_Timer / f_DelayInvu);

        for (int i = 0; i < go_ShipImpactedByColor.Count; i++)
        {
            var material = go_ShipImpactedByColor[i].GetComponent<Renderer>().material;
            material.color = new Color(1, newColorMaterial, newColorMaterial);
        }
    }

    // Method called by other object when they touch the ship (Monster, Obstacles, Boss)
    public void TriggerInvuFrame()
    {
        if (!b_InvuFrame)
        {
            // Reduce the life by 1
            GameInfo.instance.DecreaseHealth(1);

            // We create an invulnerability frame during X seconds
            b_InvuFrame = true;
            f_TimerInvu = 0;
        }
    }
}
