using System;
using System.Collections;
using UnityEngine;

public class Styx_Arm : Obstacles
{
    // Variable linked to the Arm Movement
    float f_TimerToMove = 0;
    float f_DelayToMove = 2;
    bool b_WasOnTop = false;

    // Variable linked to the Remove Part
    float f_TimerToPutBackOnTop = 0;
    float f_TimerTurnArm = 0;
    float f_TargetAngleFaceCamera = 90;
    float f_TimerToFalling = 0;
    float f_DelayRemove = .5f;
    bool b_CanFall = false;
    Vector3 v3_rotationBeforeRemoving;

    void OnEnable()
    {
        // StartCoroutine to let the time to be put in the correct place then we launch the position of the hand
        StartCoroutine(PlaceTheArm());  
    }

    // Coroutine that will place the Arm correctly regarding the position of it during the creation of the sea
    private IEnumerator PlaceTheArm()
    {
        yield return new WaitForSeconds(.5f);
        if (this.transform.localPosition.x == -GameConstante.I_BORDERX)
        {
            this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (this.transform.localPosition.x == GameConstante.I_BORDERX)
        {
            this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            int rng = UnityEngine.Random.Range(0, 1);
            if (rng == 0)
            {
                this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        if (this.transform.localEulerAngles.y == 180)
            f_TargetAngleFaceCamera = 90;
        else if (this.transform.localEulerAngles.y == 0)
            f_TargetAngleFaceCamera = -90;
    }

    // Method that manage the movement of the Arm going Up and Down
    protected override void AttackShip()
    {
        Vector3 v3_rotation = this.transform.localEulerAngles;

        if (b_WasOnTop)
        {
            v3_rotation.z = Tweening.Lerp(ref f_TimerToMove, f_DelayToMove, 0, -90);

            if (v3_rotation.z == -90)
                b_WasOnTop = false;
        }
        else
        {
            v3_rotation.z = Tweening.Lerp(ref f_TimerToMove, f_DelayToMove, -90, 0);

            if (v3_rotation.z == 0)
                b_WasOnTop = true;
        }

        this.transform.localRotation = Quaternion.Euler(v3_rotation);
    }

    // Check the collision with the Ship
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
            b_CanBeRemove = true;

        if (other.CompareTag("Ship"))
            GameObject.Find("Boss").GetComponent<BossManager>().IncreaseNoise(6);

        if (other.CompareTag("Bullet") || other.CompareTag("Ship"))
        {
            v3_rotationBeforeRemoving = this.transform.localEulerAngles;
            this.GetComponent<BoxCollider>().enabled = false;
        }

        base.OnTriggerEnter(other);
    }

    // Here the list of all movement performed by the Arm
    // Put the Arm on Top, Rotation to be face of the camera, Falling on the back and sink
    protected override void RemoveObstacle()
    {
        if (!b_CanFall)
        {
            Vector3 newRotation = this.transform.localEulerAngles;

            // First step Arm face Camera
            if (f_TimerTurnArm < f_DelayRemove)
            {
                f_TimerTurnArm += Time.deltaTime;
                newRotation.y = Mathf.Lerp(v3_rotationBeforeRemoving.y, v3_rotationBeforeRemoving.y + f_TargetAngleFaceCamera, f_TimerTurnArm / f_DelayRemove);
            }

            //  Second step Arm on Top
            if (f_TimerToPutBackOnTop < f_DelayRemove)
            {
                f_TimerToPutBackOnTop += Time.deltaTime;
                newRotation.z = Mathf.Lerp(v3_rotationBeforeRemoving.z, 270, f_TimerToPutBackOnTop / f_DelayRemove);
            }

            this.transform.localEulerAngles = newRotation;

            if (f_TimerTurnArm > f_DelayRemove && f_TimerToPutBackOnTop > f_DelayRemove)
            {
                v3_rotationBeforeRemoving = this.transform.localEulerAngles;
                b_CanFall = true;
            }
        }
        else
        {
            if (f_TimerToFalling < f_DelayRemove)
            {
                Vector3 newRotation = this.transform.localEulerAngles;
                newRotation.z = Tweening.Lerp(ref f_TimerToFalling, f_DelayRemove, v3_rotationBeforeRemoving.z, v3_rotationBeforeRemoving.z - 90);

                this.transform.localEulerAngles = newRotation;
            }
            else
            {
                ResetObstacle();
            }

        }
    }

    public override void ResetObstacle()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(GameObject.Find("NotUsed/_Obstacles").transform);
        this.GetComponent<BoxCollider>().enabled = true;

        this.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
        f_TimerToPutBackOnTop = 0;
        f_TimerToFalling = 0;


        b_CanBeRemove = false;
    }
}