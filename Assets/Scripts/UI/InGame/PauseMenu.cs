using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        GameInfo.instance.TriggerOnPause += UpdatePauseMenu;
    }

    private void UpdatePauseMenu()
    {
        if (GameInfo.instance.IsGameOnPause())
        {
            
        }
    }
}