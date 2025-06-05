using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject go_PortalPrefab;

    private float f_GapPortalBehind = 50;
    private float f_SizeLastPortal = 100;

    // Method that add the 3 portal on the scene (2 to change the region and 1 to stay on it)
    public void AddPortalOnSea(GameObject go_newSea)
    {
        Transform ItemZone = go_newSea.transform.GetChild(5).GetChild(0);

        // Check if an item is already instantiate on the zone, if so, we remove it
        if (ItemZone.childCount > 0)  GetComponent<ItemManager>().RemoveItems(go_newSea);

        // First we create the Portal on the left
        GameObject go_firstPortal = Instantiate(go_PortalPrefab, ItemZone);
        go_firstPortal.transform.localPosition = new(-GameConstante.I_BORDERX, go_firstPortal.transform.localPosition.y, go_firstPortal.transform.localPosition.z);

        RegionScriptableObject portailToRegion = RegionSelection(-1);
        go_firstPortal.GetComponent<Portal>().SettingsRegion(portailToRegion);
        go_firstPortal.GetComponent<Renderer>().material.color = portailToRegion.seaColor;

        // then we create the Portal on the Right
        GameObject go_SecondPortal = Instantiate(go_PortalPrefab, ItemZone);
        go_SecondPortal.transform.localPosition = new(GameConstante.I_BORDERX, go_firstPortal.transform.localPosition.y, go_firstPortal.transform.localPosition.z);

        portailToRegion = RegionSelection((int)portailToRegion.typeRegion);
        go_SecondPortal.GetComponent<Portal>().SettingsRegion(portailToRegion);
        go_SecondPortal.GetComponent<Renderer>().material.color = portailToRegion.seaColor;

        // Now we create the Last Portal on the middle, wider and little bit after the 2 other
        GameObject go_thirdPortal = Instantiate(go_PortalPrefab, ItemZone);
        go_thirdPortal.transform.localPosition = new(go_thirdPortal.transform.localPosition.x, go_thirdPortal.transform.localPosition.y, f_GapPortalBehind);
        go_thirdPortal.GetComponent<Portal>().SettingsRegion(GameInfo.instance.GetCurrentRegion());
        // To finish, we remove the renderer of this portal and we make it wider 
        go_thirdPortal.GetComponent<Renderer>().enabled = false;                                                            
        go_thirdPortal.transform.localScale = new(f_SizeLastPortal, go_PortalPrefab.transform.localScale.y, go_PortalPrefab.transform.localScale.z);
    }


    // Method to select the region that will be linked to the portal
    private RegionScriptableObject RegionSelection(int indexRegionAlreadySelected)
    {
        // Loop to generate the random selection
        for (int i = 0; i < 100; i++)
        {
            int indexSelectedRegion = Random.Range(0, GameInfo.instance.GetListRegion().Count);

            if (indexSelectedRegion != (int)GameInfo.instance.GetCurrentRegion().typeRegion && indexSelectedRegion != indexRegionAlreadySelected)
            {
                return GameInfo.instance.GetListRegion()[indexSelectedRegion];
            }
        }

        return GameInfo.instance.GetListRegion()[0];
    }
}