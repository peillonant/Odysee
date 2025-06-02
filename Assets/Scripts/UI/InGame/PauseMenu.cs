using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        GameInfo.TriggerOnPause += UpdatePauseMenu;
    }

    private void UpdatePauseMenu()
    {
        if (GameInfo.IsGameOnPause())
        {
            
        }
    }
}