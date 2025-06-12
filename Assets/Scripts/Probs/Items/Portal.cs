using UnityEngine;

public class Portal : Items
{

    private GameObject go_Sea;
    private GameObject go_Boss;
    private RegionScriptableObject regionLinked;
    private bool b_newRegion = true;

    void Start()
    {
        go_Sea = GameObject.Find("Sea");
        go_Boss = GameObject.Find("Boss");
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            go_Sea.GetComponent<ItemManager>().RemoveItems(this.transform.parent.parent.parent.gameObject);

            // Set previous and current Region
            if (b_newRegion)
            {
                GameInfo.instance.SetPreviousRegion();
                GameInfo.instance.SetCurrentRegion(regionLinked);
                go_Sea.GetComponent<DisplayManager>().TriggerChangeDisplay();
            }

            // Reset of value currentregion to trigger the new region
            TriggerChangeRegion();

            base.DestroyIt();
        }
    }

    private void TriggerChangeRegion()
    {
        // Update the Distance to be set back to 0 on the new region
        GameInfo.instance.ResetDistance();

        // Reset the BossManager to be able to trigger the boss on the new region
        go_Boss.GetComponent<BossManager>().ResetBossManager();

        // Trigger the method on SeaManager to be able to generate Obstacles and Monster again
        go_Sea.GetComponent<SeaManager>().CanTriggerProbs();
    }

    public void SettingsRegion(RegionScriptableObject portailToRegion)
    {
        if (portailToRegion == GameInfo.instance.GetCurrentRegion())
            b_newRegion = false;

        regionLinked = portailToRegion;
    }
}