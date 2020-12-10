using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Formations;

public class GameRules : MonoBehaviour
{

    [SerializeField]
    public StartFormation StartFormation;

    [SerializeField]
    [Range(1,2)]
    public int BallsBackCount = 2;

    [SerializeField]
    [Range(1, 3)]
    public int Restacks = 1;

    [SerializeField]
    [Range(1, 4)]
    public int SameCupCount = 2;

}
