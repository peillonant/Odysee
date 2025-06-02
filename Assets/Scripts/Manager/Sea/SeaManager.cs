using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SeaManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> go_SeaPrefab = new();
    private GameObject go_SeaNotUsed;
    private GameObject go_Ship;

    private int i_MinPrefabSelected = 0;
    private int i_MaxPrefabSelected = 3;

    void Start()
    {
        go_SeaNotUsed = GameObject.Find("NotUsed/Sea");
        go_Ship = GameObject.Find("Ship");
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameInfo.IsGameLost() && !GameInfo.IsGameOnPause())
            CheckSeaToBeRemoved();
    }

    // Method used to check if the first sea child is still "used" by the ship of player before being removed
    private void CheckSeaToBeRemoved()
    {
        // First we check if the first sea available
        GameObject go_FirstSeaDisplayed = this.transform.GetChild(0).gameObject;
        float f_PositionSea = go_FirstSeaDisplayed.transform.position.z + GameConstante.I_SIZESEA / 2;

        if (go_Ship.transform.position.z > +f_PositionSea + GameConstante.I_GAPBEFOREREMOVING)
        {
            RemoveSea(go_FirstSeaDisplayed);
            AddNewSea();
        }
    }

    // Method to push the sea passed on the gameObject NotUsed/Sea that can be use again later
    private void RemoveSea(GameObject go_SeaToMove)
    {
        go_SeaToMove.SetActive(false);
        go_SeaToMove.transform.position = Vector3.zero;

        // Having a call to the Obstacle Manager to remove all obstacle linked to the GO
        this.GetComponent<ObstacleManager>().RemoveObstacles(go_SeaToMove);

        // Having a call to the Item Manager to remove all items still existing on the GO
        this.GetComponent<ItemManager>().RemoveItems(go_SeaToMove);

        // Having a call to the Monster Manager to remove all monster still existing on the GO
        this.GetComponent<MonsterManager>().RemoveMonster(go_SeaToMove);

        go_SeaToMove.transform.SetParent(go_SeaNotUsed.transform);
    }

    // Method to add the new sea on the scene. Here we will check if a gameObject with the prefab is already used or not.
    private void AddNewSea()
    {
        // First we retrieve the position of the last Sea
        float f_newPositionZ = this.transform.GetChild(this.transform.childCount - 1).position.z + GameConstante.I_SIZESEA;
        Vector3 v3_NewPositionSea = new(0, 0, f_newPositionZ);

        // Second, we create use the random generator to select the prefab that will be added to the scene
        int i_indexRng = Random.Range(i_MinPrefabSelected, i_MaxPrefabSelected + 1);

        // Third, we check if this prefab is already created and not currently used
        GameObject go_newSea = GeneralFunction.RetrieveObject(go_SeaNotUsed, go_SeaPrefab[i_indexRng], this.transform);

        if (go_newSea == null)
        {
            go_newSea = Instantiate(go_SeaPrefab[i_indexRng], this.transform);
            go_newSea.name = go_SeaPrefab[i_indexRng].name;
        }

        // Then we update the position on the scene of the first sea to be the last one
        go_newSea.transform.position = v3_NewPositionSea;

        if (!GameInfo.IsBossFight())
        {
            // To finish, we call the ObstacleManager
            this.GetComponent<ObstacleManager>().CreateObstacle(go_newSea);

            // We call the MonsterManager to know if we add new Monster on it
            this.GetComponent<MonsterManager>().CreateMonster(go_newSea);
        }

        // We call the ItemManager to know if we add new items on it
        this.GetComponent<ItemManager>().CreateItems(go_newSea);
    }

    // Method to Remove all Monster created before the trigger of the Boss
    public void RemoveObstaclesAndMonsters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go_SeaToMove = transform.GetChild(i).gameObject;

            // Having a call to the Obstacle Manager to remove all obstacle linked to the GO
            this.GetComponent<ObstacleManager>().RemoveObstacles(go_SeaToMove);

            // Having a call to the Monster Manager to remove all monster still existing on the GO
            this.GetComponent<MonsterManager>().RemoveMonster(go_SeaToMove);
        }
    }
}
