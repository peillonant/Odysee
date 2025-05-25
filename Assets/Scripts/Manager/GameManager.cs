using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameInfo.SetCurrentRegion(TypeRegion.EGEE);   
    }

    void FixedUpdate()
    {
        if (GameInfo.GetCurrentHealth() <= 0)
        {
            Debug.Log("Game loose");
            Debug.Break();
        }   
    }
}
