using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Formations;

public class GameRules : MonoBehaviour
{
    public bool LoadRulesFromFile = true;


    [SerializeField]
    public StartFormation StartFormation;

    [SerializeField]
    [Range(1,2)]
    public int BallsBackCount = 2;

    [SerializeField]
    [Range(1, 3)]
    public int Restacks = 1;

    [SerializeField]
    [Range(1, 3)]
    public int Islands = 1;

    [SerializeField]
    [Range(1, 3)]
    public int IslandsReward = 1;

    [SerializeField]
    [Range(1, 4)]
    public int SameCupReward = 2;

    public int NumberOfBallsPrRound = 2;

    private void Awake()
    {
        if (LoadRulesFromFile)
        {
            var rules = SaveSystem.LoadRules();
            ApplyRules(rules);
        }
    }

    void ApplyRules(RulesData data)
    {
        Restacks = int.Parse(data.restacks);
        StartFormation = (StartFormation)System.Enum.Parse(typeof(StartFormation), data.startFormation);
        Islands = int.Parse(data.islands);
        BallsBackCount = int.Parse(data.ballsBack);
    }

}
