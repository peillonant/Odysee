using System;
using UnityEngine;

public class GeneralFunction : MonoBehaviour
{
    // Method to check on the pool of Unused GameObject if  has been already created before to use it again
    public static GameObject RetrieveObject(GameObject go_NotUsed, GameObject go_ToCheck, Transform tr_Parent)
    {
        if (go_NotUsed.transform.childCount > 0)
        {
            for (int i = 0; i < go_NotUsed.transform.childCount; i++)
            {
                if (go_NotUsed.transform.GetChild(i).name == go_ToCheck.name)
                {
                    GameObject go_Retrieved = go_NotUsed.transform.GetChild(i).gameObject;
                    go_Retrieved.SetActive(true);
                    go_Retrieved.transform.SetParent(tr_Parent);

                    return go_Retrieved;
                }
            }
        }
    
        return null;
    }
}