using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    // Method linked to the Input System Action to trigger the pause Menu
    public void OnPressPause(InputAction.CallbackContext value)
    {
        if (value.started && !GameInfo.instance.IsGameLost())
            GameInfo.instance.TriggerPauseMenu();
    }

    // Method Linked to the Input System Action to trigger the close of the Tutoriel
    public void OnPressTab(InputAction.CallbackContext value)
    {
        if (value.started && !GameInfo.instance.TutorielHasBeenSeen())
            GameInfo.instance.SetTutorielSeen(true);

        if (value.started)
            HUDManager.instance.DisplayTutorial();
    }
}