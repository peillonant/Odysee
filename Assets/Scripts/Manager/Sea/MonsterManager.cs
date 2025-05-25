using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject go_MonsterNotUsed;
    [SerializeField] private List<GameObject> go_PrefabMonster;

    private int i_indexMonster = 0;

    private int i_ThresholdToTriggerMonster = 20;

    // Method that will update the Threshold to trigger the creation of the Monster regarding the Distance already travelled
    public void UpdateThresholdMonster()
    {
        // To do
    }

    // Method used when we change the region to update which Monster can be create on the region
    public void UpdateMonsterIndex()
    {
        switch (GameInfo.GetCurrentRegion())
        {
            case TypeRegion.EGEE:
                i_indexMonster = 0;
                break;
            case TypeRegion.STYX:
                i_indexMonster = 2;
                break;
            case TypeRegion.OLYMPUS:
                i_indexMonster = 4;
                break;
            case TypeRegion.ETNAS:
                i_indexMonster = 6;
                break;
            case TypeRegion.ARCADIE:
                i_indexMonster = 8;
                break;
        }
    }

    public void CreateMonster(GameObject go_newSea)
    {

        int i_RngCreateMonster = Random.Range(0, 20);

        // If the random is below the threshold, we do not create the Monster
        if (i_RngCreateMonster > i_ThresholdToTriggerMonster) return;

        // First we check if the Sea prefab has an Obstracle Zone.
        if (go_newSea.transform.GetChild(4).childCount > 0)
        {
            // Now retrieve the number of zone, this will allow us to generate the Monster on one Zone
            int i_NbZone = go_newSea.transform.GetChild(4).childCount;

            int i_IndexZone = Random.Range(0, i_NbZone);

            int i_IndexMonsterSelected = Random.Range(i_indexMonster, i_indexMonster + 2);  // We have to add 2 due to the Exclusivity of the Random.range()

            Transform obstacleZone = go_newSea.transform.GetChild(4).GetChild(i_IndexZone);

            GameObject go_newMonster = GeneralFunction.RetrieveObject(go_MonsterNotUsed, go_PrefabMonster[i_IndexMonsterSelected], obstacleZone);

            if (go_newMonster == null)
            {
                go_newMonster = Instantiate(go_PrefabMonster[i_IndexMonsterSelected], obstacleZone);
                go_newMonster.name = go_PrefabMonster[i_IndexMonsterSelected].name;
            }

            Vector3 v3_NewPositionMonster = new(0, go_newMonster.transform.position.y, 0);

            if (obstacleZone.GetChild(0).GetComponent<Obstacles>().GetTypeObstaclesSize() == TypeObstaclesSize.SHORT)
            {
                // We are now retrieve the position of the obstacle to avoid putting the monster on it
                float f_positionObstacle = obstacleZone.GetChild(0).position.x;

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
    }

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
                        go_ToRemove.SetActive(false);
                        go_ToRemove.transform.position = new (0, go_ToRemove.transform.position.y, 0);
                        go_ToRemove.transform.SetParent(go_MonsterNotUsed.transform);
                    }
                }
            }
        }
    }
}