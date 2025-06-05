using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject go_HUD;
    [SerializeField] private GameObject go_MenuOnPause;
    [SerializeField] private GameObject go_MenuLost;

    void Start()
    {
        // I'm using Event here to trigger the correct Display 
        GameInfo.instance.TriggerLost += DisplayLostMenu;
        GameInfo.instance.TriggerOnPause += DisplayOnPauseMenu;
    }

    private void DisplayLostMenu()
    {
        go_HUD.SetActive(false);
        go_MenuLost.SetActive(true);
        go_MenuLost.GetComponent<LostMenu>().UpdateLostMenu();
    }

    private void DisplayOnPauseMenu()
    {
        go_HUD.SetActive(!GameInfo.instance.IsGameOnPause());
        go_MenuOnPause.SetActive(GameInfo.instance.IsGameOnPause());
    }

}
