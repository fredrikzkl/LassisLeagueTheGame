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
    public float rotationSpeed = 1.25f;
    public float maxAngle = 0.4f;

    //Y angle stuff
    private Vector3 mousePosOrigin = Vector3.zero;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private void Awake()
    {
        body = GetComponent<Transform>();
        rotate = false;
    }


    /*
    private void Update()
    {

        if(Input.GetButtonUp("Fire1"))
        {
            mousePosOrigin = Vector3.zero;
            body.SetPositionAndRotation(originalPosition, originalRotation);
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            mousePosOrigin = Input.mousePosition;
            originalRotation = body.rotation;
            originalPosition = body.position;
        }


        if (mousePosOrigin != Vector3.zero)
        {
            var angle = MouseAimAngle();
            body.RotateAround(pivotPoint.position, new Vector3(0f, 0f, 1f), angle);
            /*
            var goal = Quaternion.Euler(new Vector3(0, 1f, angle));
            if (body.rotation != goal)
            {
                

            }
         
        }
    }
            */
    //Y - direction aim
    public float MouseAimAngle()
    {
        var finalMousePos = Input.mousePosition;
        var deltaX = finalMousePos.x - body.position.x;
        var deltaY = finalMousePos.y - body.position.y;
        var angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg; 
        return angle;
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
                //Debug.Log(delta + " | " + (maxAnglePI) + " | RotationSpeed: " + rotationSpeed);
                if ((delta < maxAnglePI && rotationSpeed > 0 && delta > 0) || (delta > -maxAnglePI && rotationSpeed < 0 && delta < 0))
                    rotationSpeed *= -1;

            }
            else
            {
                //Debug.Log(delta + " | " + (maxAngle) + " | RotationSpeed: " + rotationSpeed);
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
