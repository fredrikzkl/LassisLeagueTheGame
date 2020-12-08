using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formations 
{
    //10 kopper: Trekant
    public static string FullHouse  = "101010101#010101010#001010100#000101000#000010000";
    //6 kopper: 3 bak, full trekant
    public static string HalfHouse  = "001010100#000101000#000010000";
    //5 kopper: 3 bak, full trekant
    public static string HalfHouseNoNose = "001010100#000101000";
    //4 kopper: Diamant
    public static string Vagina = "000010000#000101000#000010000";
    //3 kopper: Trekant
    public static string Trio = "000101000#000010000"; 
    //2 på rad
    public static string Gentlemans = "000010000#000010000";
    //1 igjen
    public static string Solo = "000010000";
    
    public static string GetStandardFormation(int cups)
    {
        switch (cups)
        {
            case 6:
                return HalfHouse;
            case 5:
                return HalfHouseNoNose;
            case 4:
                return Vagina;
            case 3:
                return Trio;
            default:
                Debug.Log("No need to rerack :)");
                return "";
        }
    }

}
