using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager_Boat : MonoBehaviour
{
     // Variable linked to the Sail
    private bool b_CanMoveSail = false;
    private float f_MousePositionStartZ = 0;


    // Method linked to the Input System Action to move the boat on the correct lane
    public void OnMovement(InputAction.CallbackContext value)
    {
        if (value.started && !GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            Vector3 v3_tmpImputMovement = new(value.ReadValue<Vector2>().x, 0, 0);

            GetComponent<ShipController>().UpdateTargetPosition(v3_tmpImputMovement);
        }
    }

    // Method linked to the Input System Action to know when the Sail can be rotate or not
    public void CanMoveSail(InputAction.CallbackContext value)
    {
        if (!GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            b_CanMoveSail = value.performed;

            if (value.started)
                f_MousePositionStartZ = 0;
        }
    }

    // Method linked to the Input System Action to compute the rotation of the sail
    public void OnRotateSail(InputAction.CallbackContext value)
    {
        if (b_CanMoveSail && !GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            if (f_MousePositionStartZ == 0)
                f_MousePositionStartZ = value.ReadValue<Vector2>().x;

            GetComponent<ShipController>().TriggerRotationSail(f_MousePositionStartZ, value.ReadValue<Vector2>().x);
        }
    }

    // Method linked to the Input System Action to trigger the canon
    public void OnPressCanon(InputAction.CallbackContext value)
    {
        if (value.started && !GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
            GetComponent<CannonController>().TriggerCanon();
    }
}