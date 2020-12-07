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
        
        if (rotate)
        {
            var delta = GetAngle();
           
            if (direction == 1)
            {
                 if ((delta > maxAngle && rotationSpeed > 0) || (delta < -maxAngle && rotationSpeed < 0))
                    rotationSpeed = rotationSpeed * -1;
            }
            else
            {
                //Debug.Log(delta + " | " + maxAngle + " Direction: " + rotationSpeed);
                if((delta < -maxAngle && rotationSpeed > 0) || (delta > maxAngle && rotationSpeed < 0))
                    rotationSpeed = rotationSpeed * -1;

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
        float deltaX;
        float deltaY = pivotPoint.position.z - body.position.z;

        if (direction == 1)
        {
            deltaX = pivotPoint.position.x - body.position.x;
        }
        else
        {
            deltaX = -(pivotPoint.position.x - body.position.x);
        }
    
        float degree = Mathf.Atan2(deltaY, deltaX);

        return -degree;
    }

    public static float normalizeAngle(float angle)
    {
        return Mathf.Atan2(Mathf.Sin(angle), Mathf.Cos(angle));
    }


}
