using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay 
{
    public Vector2 arc;
    public float angle;
    public float power;
    public Transform originPosition;

    public Replay(Vector2 arc, float angle, float power, Transform originPosition)
    {
        this.arc = arc;
        this.angle = angle;
        this.power = power;
        this.originPosition = originPosition;
    }
        
}
