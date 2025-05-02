using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class ShipController : MonoBehaviour
{
    private const int I_BORDERX = 35;

    public float f_currentSpeed = 10;
    private Vector3 v3_targetPosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePlayerMovement();
        GoingForward();
    }

    // Method linked to the Update of the Ship
    void UpdatePlayerMovement()
    {
        // Update the target Position to create the animation of the boat floating toward the lane
        if (GameInfo.GetCurrentRegion() == TypeRegion.OLYMPUS)
            v3_targetPosition = new (v3_targetPosition.x, v3_targetPosition.y, this.transform.position.z);
        else
            v3_targetPosition = new (v3_targetPosition.x, this.transform.position.y, this.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, v3_targetPosition, f_currentSpeed * Time.deltaTime);
    }

    // Method to move forward the boat
    void GoingForward()
    {
        Vector3 v3_tmpPosition = transform.position;

        v3_tmpPosition.z += f_currentSpeed * Time.deltaTime * 6; 

        transform.position = v3_tmpPosition;
    }

    // Function linked to the Input System Action
    public void OnMovement(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Vector3 v3_tmpImputMovement;

            if (GameInfo.GetCurrentRegion() == TypeRegion.OLYMPUS)
                v3_tmpImputMovement = (Vector3) value.ReadValue<Vector2>();
            
            else
                v3_tmpImputMovement = new (value.ReadValue<Vector2>().x, 0, 0);

            UpdateTargetPosition(v3_tmpImputMovement);
        }
    }

    private void UpdateTargetPosition(Vector3 v3_InputMovement)
    {
        if (GameInfo.GetCurrentRegion() == TypeRegion.OLYMPUS)
        {
            // We have to update the position of the X and the Y
        }
        else if (GameInfo.GetCurrentRegion() == TypeRegion.ETNAS)
        {
            // We have to update the position with more than 3 lanes (probably 5)
        }
        else
        {
            if (v3_InputMovement.x < 0)                                                                 // The player wants to move the boat to the left lane
            {
                if (v3_targetPosition.x == 0)                                                           // We are on the middle lane
                    v3_targetPosition = new( -I_BORDERX, v3_targetPosition.y, v3_targetPosition.z);
                else if (v3_targetPosition.x == I_BORDERX)                                              // We are on the right lane
                    v3_targetPosition = new( 0, v3_targetPosition.y, v3_targetPosition.z);
            }
            else if (v3_InputMovement.x > 0)                                                            // The player wants to move the boat to the rigth lane
            {
                if (v3_targetPosition.x == 0)                                                           // We are on the middle lane
                    v3_targetPosition = new( I_BORDERX, v3_targetPosition.y, v3_targetPosition.z);
                else if (v3_targetPosition.x == -I_BORDERX)                                             // We are on the left lane
                    v3_targetPosition = new( 0, v3_targetPosition.y, v3_targetPosition.z);
            }
        }
    }
}
