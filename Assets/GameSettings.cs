using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    public bool ShowFps;

    [SerializeField]
    [Range(50f, 250f)]
    public float VerticalAimSensitivity = 100f;
}
