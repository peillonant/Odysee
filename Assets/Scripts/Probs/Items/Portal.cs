using UnityEngine;

public class Portal : Items
{
    private RegionScriptableObject regionLinked;
    private bool b_newRegion = true;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            GameObject.Find("Sea").GetComponent<ItemManager>().RemoveItems(this.transform.parent.parent.parent.gameObject);

            // Set previous and current Region
            if (b_newRegion)
            {
                GameInfo.instance.SetPreviousRegion();
                GameInfo.instance.SetCurrentRegion(regionLinked);
            }
        
            // Reset of value currentregion to trigger the new region

            base.DestroyIt();
        }
    }

    public void SettingsRegion(RegionScriptableObject portailToRegion)
    {
        if (portailToRegion == GameInfo.instance.GetCurrentRegion())
            b_newRegion = false;

        regionLinked = portailToRegion;
    }
}