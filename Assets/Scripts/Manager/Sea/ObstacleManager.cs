using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject go_ObstaclesNotUsed;

    // Method called by the SeaManager to create/retrieve an obstacle and add it on the sea created
    public void CreateObstacle(GameObject go_newSea)
    {
        // First we check if the Sea prefab has an Obstracle Zone.
        if (go_newSea.transform.GetChild(4).childCount > 0)
        {
            // Now retrieve the number of zone, to generate an obstacle on every area
            int i_NbZone = go_newSea.transform.GetChild(4).childCount;

            // We launch the creation of the obstacles on each zone available
            for (int i = 0; i < i_NbZone; i++)
            {
                int i_IndexObstacles = Random.Range(0, GameInfo.instance.GetCurrentRegion().obstaclesPrefab.Count);  // We have to add 2 due to the Exclusivity of the Random.range()

                // With this methode, either we retrieve a GameObject created before and not used or we create a new GameObject
                GameObject go_newObstacle = GeneralFunction.RetrieveObject(go_ObstaclesNotUsed, GameInfo.instance.GetCurrentRegion().obstaclesPrefab[i_IndexObstacles], go_newSea.transform.GetChild(4).GetChild(i));

                if (go_newObstacle == null)
                {
                    go_newObstacle = Instantiate(GameInfo.instance.GetCurrentRegion().obstaclesPrefab[i_IndexObstacles], go_newSea.transform.GetChild(4).GetChild(i));
                    go_newObstacle.name = GameInfo.instance.GetCurrentRegion().obstaclesPrefab[i_IndexObstacles].name;
                }

                Vector3 v3_NewPositionObstacle = new(0, go_newObstacle.transform.position.y, 0);

                // Then we update the position on the scene
                if (go_newObstacle.GetComponent<Obstacles>().GetTypeObstaclesSize() == TypeObstaclesSize.SHORT)
                {
                    // If it's short, we have to select a lane (0 - Left, 1 - Middle, 2 - Right)        /!\ will need an update when we will have other region with more lane
                    int i_IndexLane = Random.Range(0, 3);

                    switch (i_IndexLane)
                    {
                        case 0:
                            v3_NewPositionObstacle.x = -GameConstante.I_BORDERX;
                            break;
                        case 1:
                            v3_NewPositionObstacle.x = 0;
                            break;
                        case 2:
                            v3_NewPositionObstacle.x = GameConstante.I_BORDERX;
                            break;
                    }
                }
                else if (go_newObstacle.GetComponent<Obstacles>().GetTypeObstaclesSize() == TypeObstaclesSize.BIG)
                {
                    // If it's long, we have to select if the obstacle is blocking Left-Middle (0) or Middle-Right (1)
                    int i_IndexLane = Random.Range(0, 2);

                    switch (i_IndexLane)
                    {
                        case 0:
                            v3_NewPositionObstacle.x = 0;
                            break;
                        case 1:
                            v3_NewPositionObstacle.x = GameConstante.I_BORDERX;
                            break;
                    }
                }

                go_newObstacle.transform.localPosition = v3_NewPositionObstacle;

            }
        }
    }

    // Method called by the SeaManager to put obtacles still display on the gameobject unused for later
    public void RemoveObstacles(GameObject go_RemovedSea)
    {
        // First we check if the Sea prefab has an Obstracle Zone.
        if (go_RemovedSea.transform.GetChild(4).childCount > 0)
        {
            // Now we check on every area if an obstacle is display. If so, we remove it and put it on the Unused gameObject 
            for (int i = 0; i < go_RemovedSea.transform.GetChild(4).childCount; i++)
            {
                if (go_RemovedSea.transform.GetChild(4).GetChild(i).childCount > 0 && go_RemovedSea.transform.GetChild(4).GetChild(i).GetChild(0).CompareTag("Obstacle"))
                {
                    GameObject go_ToRemove = go_RemovedSea.transform.GetChild(4).GetChild(i).GetChild(0).gameObject;
                    go_ToRemove.SetActive(false);
                    go_ToRemove.transform.position = new(0, go_ToRemove.transform.position.y, 0);
                    go_ToRemove.transform.SetParent(go_ObstaclesNotUsed.transform);
                }
            }
        }
    }
}
