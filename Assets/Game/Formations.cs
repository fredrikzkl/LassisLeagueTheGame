using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formations 
{
    public enum StartFormation
    {
        Triangle, SmallTriangle, Giga 
    }

    public class Formation
    {
        public string Name { get; }
        public int Cups { get; }
        public string FormationString { get; }
        public Formation(string name, int cups, string formationString)
        {
            Name = name;
            Cups = cups;
            FormationString = formationString;

        }
    }

    //https://www.dimensions.com/element/beer-pong-racks-re-racks
    //15 kopper: Mega boy
    public static Formation GigaHouse  = new Formation("Giga House", 15, "101010101#010101010#001010100#000101000#000010000");
    //10 - Standard start
    public static Formation Triangle = new Formation("Triangle", 10,"01010101#001010100#000101000#000010000");
    //8 Marching
    public static Formation Marching = new Formation("North Korean Army", 8,"000101000#000101000#000101000");
    //7 kopper: 4-3 bak, full trekant
    public static Formation HoneyComb = new Formation("Honning", 7, "000101000#001010100#000101000");
    public static Formation TraingleWithClit = new Formation("Triangle med nipple", 7, "001010100#000101000#000010000#000101000");
    //6 kopper: 3 bak, full trekant
    public static Formation SmallTriangle  = new Formation("Liten triangel", 6, "001010100#000101000#000010000");
    public static Formation Zipper = new Formation("Zipper", 6, "000100000#000010000#000100000#000010000#000100000#000010000");
    //5 kopper: 3 bak, full trekant
    public static Formation Trapezoid = new Formation("Trapezoid",5,"001010100#000101000");
    public static Formation House = new Formation("Hus", 5,"000101000#000101000#000010000");
    //4 kopper: Diamant
    public static Formation Vagina = new Formation("Vagina", 4,"000010000#000101000#000010000");
    public static Formation DickNBalls = new Formation("Dick'n'balls",4,"000101000#000010000#000010000");
    //3 kopper: Trekant
    public static Formation Trio = new Formation("Trio", 3,"000101000#000010000"); 
    //2 på rad
    public static Formation Gentlemans = new Formation("Gentlemans", 2, "000010000#000010000");
    //1 igjen
    public static Formation Solo = new Formation("Solo",1,"000010000");

    
    public static List<Formation> GetValidFormations(int cups)
    {
        List<Formation> formations = new List<Formation>();
        switch (cups)
        {
            case 10:
                formations.Add(Triangle);
                break;
                
            case 8:
                formations.Add(Marching);
                break;
            case 7:
                formations.Add(HoneyComb);
                formations.Add(TraingleWithClit);
                break;
            case 6:
                formations.Add(SmallTriangle);
                formations.Add(Zipper);
                break;
            case 5:
                formations.Add(Trapezoid);
                formations.Add(House);
                break;
            case 4:
                formations.Add(Vagina);
                formations.Add(DickNBalls);
                break;
            case 3:
                formations.Add(Trio);
                break;
            case 2:
                break;
            case 1:
                formations.Add(Solo);
                break;
            default:
                break;
        }
        return formations;
    }

    public static string GetStartFormation(StartFormation sf)
    {
        switch (sf)
        {
            case StartFormation.Triangle:
                Debug.Log(Triangle.FormationString);
                return Triangle.FormationString;
            case StartFormation.SmallTriangle:
                return SmallTriangle.FormationString;
            case StartFormation.Giga:
                return GigaHouse.FormationString;
            default:
                return Triangle.FormationString;
        }
    }
    

}
