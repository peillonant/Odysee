using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> go_ListItems;
    private Vector3 v3_ItemDefaultPosition = new(0, 5, 0);

    public void RemoveItems(GameObject go_RemovedSea)
    {
        // First we check if the Sea prefab has still GO Item
        if (go_RemovedSea.transform.GetChild(5).childCount > 0)
        {
            GameObject go_ItemParent = go_RemovedSea.transform.GetChild(5).gameObject;

            for (int i = 0; i < go_ItemParent.transform.childCount; i++)
            {
                GameObject go_ZoneItem = go_ItemParent.transform.GetChild(i).gameObject;

                if (go_ZoneItem.transform.childCount > 0)
                {
                    for (int j = 0; j < go_ZoneItem.transform.childCount; j++)
                    {
                        go_ZoneItem.transform.GetChild(j).GetComponent<Items>().DestroyIt();
                    }
                }           
            }
        }
    }


    // Methode regarding the generation of Items
    // We use a random between 0 and 10, with the index 0 for the Oboles and the rest for the Ammo
    public void CreateItems(GameObject go_newSea)
    {
        // If it's Boss Fight, we are not able to generate Oboles
        int i_MinValue = GameInfo.instance.IsBossFight() ? 1 : 0;

        int i_IndexRngItem = Random.Range(i_MinValue, 11);

        if (i_IndexRngItem == 0)
            GenerateOboles(go_newSea);
        else
            GenerateAmmo(go_newSea, i_IndexRngItem);
    }

    private void GenerateOboles(GameObject go_newSea)
    {
        int i_NbItemZone = go_newSea.transform.GetChild(5).childCount;

        // Now we need to know where to put the Ammo regarding obstacle Zone (Close to 0 we are on the front, higher we are on the back)
        int i_IndexPosition = Random.Range(0, i_NbItemZone);

        InstantiateItem(go_newSea, go_ListItems[0], i_IndexPosition);
    }


    // Munition (generation availalbe even if the boss fight is currently on)
    //  - If the munition available is :
    //     - 10 - 8 : Chance to spawn 1/10
    //     - 7 - 6 : Chance to spawn 3/10
    //     - 5 - 3 : Chance to spawn 6/10
    //     - 2 - 0 : Chance to spawn 9/10
    // - Moreover, we can also have a double spawn of munition regarding the current munition available
    //     - > 5 : 0/10
    //     - 5 - 3 : 5/10 to have a double spawn
    //     - 2 - 0 : 8/10 to have a double spawn
    private void GenerateAmmo(GameObject go_newSea, int i_IndexRngItem)
    {
        bool b_SpawnFirstAmmo = false;
        bool b_SpawnDoubleAmmo = false;
        int i_NbItemZone = go_newSea.transform.GetChild(5).childCount;

        // Switch to know if we are spawning 1 Ammo or 2 regarding the rules above
        switch (GameInfo.instance.GetNbAmmo())
        {
            case >= 8:
                b_SpawnFirstAmmo = i_IndexRngItem == 1;
                break;
            case >= 6:
                b_SpawnFirstAmmo = i_IndexRngItem <= 3;
                break;
            case >= 3:
                b_SpawnFirstAmmo = i_IndexRngItem <= 6;
                if (i_NbItemZone > 1) b_SpawnDoubleAmmo = i_IndexRngItem <= 5;
                break;
            case >= 0:
                b_SpawnFirstAmmo = i_IndexRngItem <= 9;
                if (i_NbItemZone > 1) b_SpawnDoubleAmmo = i_IndexRngItem <= 8;
                break;
        }

        int i_IndexPositionAmmo1 = -1;


        // We can spawn 1 Ammo
        if (b_SpawnFirstAmmo)
        {
            // Now we need to know where to put the Ammo regarding obstacle Zone (Close to 0 we are on the front, higher we are on the back)
            i_IndexPositionAmmo1 = Random.Range(0, i_NbItemZone);

            InstantiateItem(go_newSea, go_ListItems[1], i_IndexPositionAmmo1);
        }

        // We can spawn the second Ammo
        if (b_SpawnDoubleAmmo)
        {
            // Now we need to know where to put the Ammo regarding obstacle Zone (Close to 0 we are on the front, higher we are on the back)
            int i_IndexPositionAmmo2 = Random.Range(0, i_NbItemZone);
            int cpt = 0;

            while (i_IndexPositionAmmo2 == i_IndexPositionAmmo1)
            {
                i_IndexPositionAmmo2 = Random.Range(0, i_NbItemZone);
                cpt++;

                if (cpt > 10)
                    return;
            }

            InstantiateItem(go_newSea, go_ListItems[1] ,i_IndexPositionAmmo2);
        }
    }

    // Methode to Instantiate Ammo on the Scene
    private void InstantiateItem(GameObject go_newSea, GameObject go_PrefabItem, int i_IndexPosition)
    {
        // Generate on which lane we will spawn the item (0 - Left, 1 - Middle, 2 - Right)
        int i_IndexLane = Random.Range(0, 3);
        Vector3 v3_PositionItem = v3_ItemDefaultPosition;

        switch (i_IndexLane)
        {
            case 0:
                v3_PositionItem.x = -GameConstante.I_BORDERX;
                break;
            case 2:
                v3_PositionItem.x = GameConstante.I_BORDERX;
                break;
        }


        Transform go_ParentItem = go_newSea.transform.GetChild(5).GetChild(i_IndexPosition);

        // Set the parent
        GameObject go_newItem = Instantiate(go_PrefabItem, go_ParentItem);
        go_newItem.transform.localPosition = v3_PositionItem;

    }
}