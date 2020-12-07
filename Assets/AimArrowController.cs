using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrowController : MonoBehaviour
{

    private bool rotate;
    //Rotation: 1 = høyre, -1 venstre
    public int direction;
    private Transform body;
    public Transform pivotPoint;
    //Knobs
    public float rotationSpeed = 1.5f;
    public float maxAngle = 0.6f;

    private void Awake()
    {
        body = GetComponent<Transform>();
        rotate = false;

    }



    public void FixedUpdate()
    {

        //Direction > 0 = nedover
        if (rotate)
        {
            var delta = GetAngle();

            if (direction == 1)
            {
                var maxAnglePI = Mathf.PI - maxAngle;
                Debug.Log(delta + " | " + (maxAnglePI) + " | RotationSpeed: " + rotationSpeed);
                if ((delta < maxAnglePI && rotationSpeed > 0 && delta > 0) || (delta > -maxAnglePI && rotationSpeed < 0 && delta < 0))
                    rotationSpeed *= -1;

            }
            else
            {
                Debug.Log(delta + " | " + (maxAngle) + " | RotationSpeed: " + rotationSpeed);
                if ((delta < -maxAngle && rotationSpeed > 0) || (delta > maxAngle && rotationSpeed < 0))
                    rotationSpeed *= -1;
            }

            body.RotateAround(pivotPoint.position, new Vector3(0f, 1f, 0f), rotationSpeed);
        }
    }

    public void StartRotating()
    {
        rotate = true;
    }

    public float StopRotating()
    {
        rotate = false;
        return GetAngle();
    }

    private float GetAngle()
    {
        float deltaX = body.position.x - pivotPoint.position.x;
        float deltaY = body.position.z - pivotPoint.position.z;


        float degree = Mathf.Atan2(deltaY, deltaX);

        return degree;
    }

    public static float normalizeAngle(float angle)
    {
        return Mathf.Atan2(Mathf.Sin(angle), Mathf.Cos(angle));
    }


}
