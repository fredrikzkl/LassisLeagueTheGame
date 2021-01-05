using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RulesCanvas : MonoBehaviour
{

    public TMP_Dropdown startFormation;
    public TMP_Dropdown restacks;


    public void Open()
    {
        RulesData d = SaveSystem.LoadRules();

        ApplyRulesToCanvas(d);
    }
    
    public void Close()
    {
        GetAndSaveHouseRules();
    }



    void ApplyRulesToCanvas(RulesData data)
    {
        startFormation.value = startFormation.options.FindIndex(option => option.text == data.startFormation);
        restacks.value = restacks.options.FindIndex(option => option.text == data.restacks);
    }

    void GetAndSaveHouseRules()
    {
        RulesData rd = new RulesData();

        rd.startFormation = startFormation.options[startFormation.value].text;;
        rd.restacks = restacks.options[restacks.value].text;

        SaveSystem.SaveRules(rd);
    }

}
