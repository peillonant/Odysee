using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionScriptableObject", menuName = "Scriptable Objects/RegionScriptableObject")]
public class RegionScriptableObject : ScriptableObject
{
    public TypeRegion typeRegion;
    public Color seaColor;
    public Color laneColor;
    public Color cameraColor;
    public List<GameObject> obstaclesPrefab;
    public List<GameObject> monsterPrefab;
    public GameObject bossPrefab;
    public Material rewardMaterial;
}
