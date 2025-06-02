using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool b_triggerBoss = false;

    void Start()
    {
        if (GameInfo.IsGameLost())
            GameInfo.ResetGame();
    }
}
