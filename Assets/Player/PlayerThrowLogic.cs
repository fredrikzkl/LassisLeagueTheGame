using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowLogic : MonoBehaviour
{

    float powerReducerConstant = 0.075f;
    float powerReducerConstant3D = 0.1f;

    float GetAdjustedPower(float P)
    {
        return P * powerReducerConstant3D;
    }


    //Returns til VelocityVector when all 3 angles are considered
    //https://mathinsight.org/spherical_coordinates
    public Vector3 SphericalCoordinates(float xyAngle, float xzAngle, float P)
    {
        //P = P * powerReducerConstant3D;

        float x = Mathf.Sin(xyAngle) * Mathf.Cos(xzAngle) * P;

        float z = Mathf.Sin(xyAngle) * Mathf.Sin(xzAngle) * P;

        float y = Mathf.Cos(xyAngle) * P;

        return new Vector3(x,y,z);
    }

    //Tenker kun på  XZ
    public Vector3 SimpleXZ(float xz, float P)
    {
        float x = Mathf.Cos(xz) * GetAdjustedPower(P);
        float z = Mathf.Sin(xz) *GetAdjustedPower(P);
        float y = GetAdjustedPower(P);
        return new Vector3(x, y, z); 
    }

    //KUN xy
    public Vector3 SimpleXY(float xy, float P)
    {
        Debug.Log("Adjusted Power: " + GetAdjustedPower(P));

        float x = Mathf.Cos(xy) * GetAdjustedPower(P);

        float y = Mathf.Sin(xy) * GetAdjustedPower(P);

        float z = 0f;

        return new Vector3(x, y, z);
    }

   
}
