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

    //Brukt for å sikte
    bool aiming;
    //float anglePointXDistance = 250f;
    float anglePointX;
    float verticalAimSentivity;
    float XYAngle;

   

    
    private void Update()
    {
        verticalAimSentivity = FindObjectOfType<GameSettings>().VerticalAimSensitivity;
        anglePointX = verticalAimSentivity * (direction * -1);


        if (Input.GetButtonUp("Fire1"))
        {
            aiming = false;
            //body.SetPositionAndRotation(originalPosition, originalRotation);
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            mousePosOrigin = Input.mousePosition;
            aiming = true;
            originalPosition = body.position;
            originalRotation = body.rotation;
        }

        if (aiming)
        {
            CalculateXYAngle();
            //body.rotation = Quaternion.AngleAxis(RelativeXYAngle(), body.position);
        }

    }

    void CalculateXYAngle()
    {
        var tempMousePosition = Input.mousePosition;
        var deltaX = verticalAimSentivity;
        var deltaY = mousePosOrigin.y - tempMousePosition.y;

        XYAngle = Mathf.Atan2(deltaY, deltaX);
        Debug.Log("Angle: [" + XYAngle + "]");
    }

    public float GetRelativeXYAngle()
    {
        float relativeXYAngle = (Mathf.PI / 2f) - XYAngle; //-XYAngle + (Mathf.PI/2f);
        //Debug.Log("Relative " + relativeXYAngle);
        return relativeXYAngle;                                                            
    }

    private void Awake()
    {
        body = GetComponent<Transform>();
        rotate = false;
    }



    public float GetXYAngle()
    {
        return XYAngle;
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

    public void Enable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        StartRotating();
    }

    public void Disable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StopRotating();
    }


}
