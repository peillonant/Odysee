using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject go_MonsterNotUsed;

    private int i_ThresholdToTriggerMonster = 20;

    // Method that will update the Threshold to trigger the creation of the Monster regarding the Distance already travelled
    public void UpdateThresholdMonster()
    {
        // To do
    }
    
    // Method called by the SeaManager to add Monster on the sea that has been created
    public void CreateMonster(GameObject go_newSea)
    {
        int i_RngCreateMonster = Random.Range(0, 20);

        // If the random is above the threshold, we do not create the Monster
        if (i_RngCreateMonster > i_ThresholdToTriggerMonster) return;

        // We check if the Sea prefab has an Obstacle Zone to add Monster on it.
        if (go_newSea.transform.GetChild(4).childCount == 0) return;

        // Now retrieve the number of zone, this will allow us to generate the Monster on one Zone
        int i_NbZone = go_newSea.transform.GetChild(4).childCount;

        int i_IndexZone = Random.Range(0, i_NbZone);

        int i_IndexMonsterSelected = Random.Range(0, GameInfo.instance.GetCurrentRegion().monsterPrefab.Count);

        Transform obstacleZone = go_newSea.transform.GetChild(4).GetChild(i_IndexZone);

        GameObject go_newMonster = GeneralFunction.RetrieveObject(go_MonsterNotUsed, GameInfo.instance.GetCurrentRegion().monsterPrefab[i_IndexMonsterSelected], obstacleZone);

        if (go_newMonster == null)
        {
            go_newMonster = Instantiate(GameInfo.instance.GetCurrentRegion().monsterPrefab[i_IndexMonsterSelected], obstacleZone);
            go_newMonster.name = GameInfo.instance.GetCurrentRegion().monsterPrefab[i_IndexMonsterSelected].name;
        }

        Vector3 v3_NewPositionMonster = new(0, go_newMonster.transform.position.y, 0);

        if (obstacleZone.GetChild(0).GetComponent<Obstacles>().GetTypeObstaclesSize() == TypeObstaclesSize.SHORT)
        {
            // We are now retrieve the position of the obstacle to avoid putting the monster on it
            float f_positionObstacle = obstacleZone.GetChild(0).localPosition.x;

            int i_indexPosition = Random.Range(0, 2);

            if (f_positionObstacle == -GameConstante.I_BORDERX)
                v3_NewPositionMonster.x = (i_indexPosition == 0) ? 0 : GameConstante.I_BORDERX;

            else if (f_positionObstacle == 0)
                v3_NewPositionMonster.x = (i_indexPosition == 0) ? -GameConstante.I_BORDERX : GameConstante.I_BORDERX;

            else if (f_positionObstacle == GameConstante.I_BORDERX)
                v3_NewPositionMonster.x = (i_indexPosition == 0) ? -GameConstante.I_BORDERX : 0;
        }
        else if (obstacleZone.GetChild(0).GetComponent<Obstacles>().GetTypeObstaclesSize() == TypeObstaclesSize.BIG)
        {
            // We are now retrieve the position of the obstacle to avoid putting the monster on it
            float f_positionObstacle = obstacleZone.GetChild(0).position.x;

            v3_NewPositionMonster.x = (f_positionObstacle == 0) ? GameConstante.I_BORDERX : -GameConstante.I_BORDERX;
        }

        go_newMonster.transform.localPosition = v3_NewPositionMonster;
    }

    // Method called by SeaManager to put all the Monster still on the sea when go off screen to the GameObject unused to be use later
    public void RemoveMonster(GameObject go_RemovedSea)
    {
        // First we check if the Sea prefab has an Obstracle Zone.
        if (go_RemovedSea.transform.GetChild(4).childCount > 0)
        {
            // Now we check on every area if a Monster is still display. If so, we remove it and put it on the Unused gameObject 
            for (int i = 0; i < go_RemovedSea.transform.GetChild(4).childCount; i++)
            {
                if (go_RemovedSea.transform.GetChild(4).GetChild(i).childCount > 0)
                {
                    GameObject go_ToRemove = go_RemovedSea.transform.GetChild(4).GetChild(i).GetChild(0).gameObject;

                    if (go_ToRemove.CompareTag("Monster"))
                    {
                        go_ToRemove.GetComponent<Obstacles>().ResetObstacle();
                    }
                }
            }
        }
    }
}