using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RulesData 
{
    public string startFormation;
    public string restacks;
   

    public RulesData GetStandardRules()
    {
        return new RulesData
        {
            restacks = "1",
            startFormation = "Trangle"
        };
    }

    override
    public string ToString()
    {
        string rules = "--Offical Houserules--\n";
        rules += "Formation: " + startFormation + "\n";
        rules += "Restacks: " + restacks + "\n";
        return rules;
    }

    
    public void FillStandardRules()
    {

    }

    public bool isCorrupt()
    {
        if (startFormation == null || restacks == null) return true;
        return false;
    }
}
