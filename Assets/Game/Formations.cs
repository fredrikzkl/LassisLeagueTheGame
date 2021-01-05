using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Formations
{
    public enum StartFormation
    {
        Standard, Small, GigaHouse
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
    public static Formation GigaHouse = new Formation("Giga House", 15, "101010101#010101010#001010100#000101000#000010000");
    //10 - Standard start
    public static Formation Standard = new Formation("Standard", 10, "01010101#001010100#000101000#000010000");
    //8 Marching
    public static Formation Marching = new Formation("North Korean Army", 8, "000101000#000101000#000101000#000101000");
    //7 kopper: 4-3 bak, full trekant
    public static Formation HoneyComb = new Formation("Honning", 7, "000101000#001010100#000101000");
    public static Formation TraingleWithClit = new Formation("Triangle med nipple", 7, "001010100#000101000#000010000#000010000");
    //6 kopper: 3 bak, full trekant
    public static Formation Small = new Formation("Liten triangel", 6, "001010100#000101000#000010000");
    public static Formation Zipper = new Formation("Zipper", 6, "000100000#000010000#000100000#000010000#000100000#000010000");
    public static Formation Lego = new Formation("Lego", 6, "000101000#000101000#000101000");
    //5 kopper: 3 bak, full trekant
    public static Formation Trapezoid = new Formation("Trapezoid", 5, "001010100#000101000");
    public static Formation House = new Formation("Hus", 5, "000101000#000101000#000010000");
    public static Formation Sword = new Formation("Sword", 5, "000010000#000101000#000010000#000010000");
    public static Formation LongDick = new Formation("Long Dick", 4, "000101000#000010000#000010000#000010000");
    //4 kopper: Diamant
    public static Formation Vagina = new Formation("Vagina", 4, "000010000#000101000#000010000");
    public static Formation DickNBalls = new Formation("Dick'n'balls", 4, "000101000#000010000#000010000");
    public static Formation Square = new Formation("Firkant", 4, "000101000#000101000");
    //3 kopper: Trekant
    public static Formation Trio = new Formation("Trio", 3, "000101000#000010000");
    //2 på rad
    public static Formation Gentlemans = new Formation("Gentlemans", 2, "000010000#000010000");
    //1 igjen
    public static Formation Solo = new Formation("Solo", 1, "000010000");


    public static List<Formation> GetValidFormations(int cups)
    {
        List<Formation> formations = new List<Formation>();
        switch (cups)
        {
            case 10:
                formations.Add(Standard);
                break;

            case 8:
                formations.Add(Marching);
                break;
            case 7:
                formations.Add(HoneyComb);
                formations.Add(TraingleWithClit);
                formations.Add(Lego);
                break;
            case 6:
                formations.Add(Small);
                formations.Add(Zipper);
                break;
            case 5:
                formations.Add(Trapezoid);
                formations.Add(House);
                formations.Add(LongDick);
                formations.Add(Sword);
                break;
            case 4:
                formations.Add(Vagina);
                formations.Add(DickNBalls);
                formations.Add(Square);
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
            case StartFormation.Standard:
                return Standard.FormationString;
            case StartFormation.Small:
                return Small.FormationString;
            case StartFormation.GigaHouse:
                return GigaHouse.FormationString;
            default:
                return Standard.FormationString;
        }
    }


    //Lager en liste over alle posisjonene koppene skal stå
    public static (List<Vector3>, List<string>) CreatePositionMatrix(string formation, Vector3 anchorPosition, int direction, float diameter)
    {
        var tightFactor = 0.865f;

        List<Vector3> positionMatrix = new List<Vector3>();
        List<string> cupNames = new List<string>();
        Vector3 tempPos = new Vector3(anchorPosition.x, anchorPosition.y, anchorPosition.z);
        tempPos.z -= (2 * diameter);
        //Decoding string:
        string[] rows = formation.Split('#');
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            var rowResult = CreatePositionMatrixRow(tempPos, row, i, diameter);
            foreach (var position in rowResult.Item1)
            {
                positionMatrix.Add(position);
            }
            foreach (var cupId in rowResult.Item2)
            {
                cupNames.Add(cupId);
            }

            var tightnigFactor = tightFactor;
            if(i + 1 < rows.Length && nextRowIsNotTight(row, rows[i+1]))
                tightnigFactor = 1;
       
            tempPos.x += (direction * diameter * tightnigFactor);
        }
        return (positionMatrix, cupNames);
    }

    static bool nextRowIsNotTight(string currentRow, string nextRow)
    {
        for(int i = 0; i < currentRow.Length; i++)
        {
            if(currentRow[i] == '1' && nextRow[i] == '1')
            {
                return true;
            }
        }
        return false;
    }

    public static (List<Vector3>, List<string>) CreatePositionMatrixRow(Vector3 pos, string row, int rowIndex, float diameter)
    {
        List<string> cupIdList = new List<string>();
        List<Vector3> rowList = new List<Vector3>();
        for (int j = 0; j < row.Length; j++)
        {
            char symbol = row[j];
            if (symbol == '1')
            {
                rowList.Add(pos);
                cupIdList.Add(rowIndex + "-" + j);
            }
            pos.z = pos.z + (0.5f * diameter);
        }
        return (rowList, cupIdList);
    }

    


    //Lager en liste over alle posisjonene koppene skal stå
    public static (List<Vector3>, List<string>) Create2DPositionMatrix(string formation, Vector3 anchorPosition, int direction, float diameter)
    {
        var tightFactor = 0.865f;

        List<Vector3> positionMatrix = new List<Vector3>();
        List<string> cupNames = new List<string>();

        Vector3 tempPos = new Vector3(anchorPosition.x, anchorPosition.y, anchorPosition.z);
        tempPos.y -= (2 * diameter);
        //Decoding string:
        string[] rows = formation.Split('#');
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];

            var rowResult = Create2DPositionMatrixRow(tempPos, row, i, diameter);
            rowResult.Item1.Reverse();

            rowResult.Item1.ForEach(coordinate => positionMatrix.Add(coordinate));
            rowResult.Item2.ForEach(name => cupNames.Add(name));
            tempPos.x += (direction * diameter * tightFactor);
        }
        return (positionMatrix, cupNames);
    }

    public static (List<Vector3>, List<string>) Create2DPositionMatrixRow(Vector3 pos, string row, int rowIndex, float diameter)
    {
        List<Vector3> rowList = new List<Vector3>();
        List<string> cupNames = new List<string>();

        for (int j = 0; j < row.Length; j++)
        {
            char symbol = row[j];
            if (symbol == '1')
            {
                rowList.Add(pos);
                cupNames.Add(rowIndex + "-" + j);
            }
            pos.y = pos.y + (0.5f * diameter);
        }
        return (rowList, cupNames);
    }


    public static NeighbourTable DetermineCupNeighbors(List<GameObject> cups, string formation)
    {
      
        NeighbourTable neighborTable = new NeighbourTable();
        string[] rows = formation.Split('#');
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            for (int j = 0; j < row.Length; j++)
            {
                if (row[j] == '1')
                {
                    List<string> neigbors = new List<string>();

                    int rowIndex = i;

                    //Sjekker samme rad
                    int colIndex = j - 2;
                    CheckNeighbor(row, colIndex, i,  neigbors);

                    colIndex = j + 2;
                    CheckNeighbor(row, colIndex, i,  neigbors);


                    //Sjekker raden forran
                    if(i+1 < rows.Length)
                    {
                        var tempRow = rows[i + 1];
                        CheckNeighbor(tempRow, j, i + 1, neigbors);
                        colIndex = j + 1;
                        CheckNeighbor(tempRow, colIndex, i + 1, neigbors);
                        colIndex = j - 1;
                        CheckNeighbor(tempRow, colIndex, i + 1, neigbors);
                    }

                    //Sjekker raden forran
                    if (i - 1 >= 0)
                    {
                        var tempRow = rows[i - 1];
                        CheckNeighbor(tempRow, j, i - 1, neigbors);
                        colIndex = j + 1;
                        CheckNeighbor(tempRow, colIndex, i - 1, neigbors);
                        colIndex = j - 1;
                        CheckNeighbor(tempRow, colIndex, i - 1, neigbors);
                    }


                    //Oppdaterer akutelle koppen
                    var tempName = i + "-" + j;

                    /*
                    Debug.Log("---Naboene til [ " + tempName + " ] ---");
                    neigbors.ForEach(n => Debug.Log(n));
                    */

                    GameObject cup = cups.Where(w => w.name == tempName).FirstOrDefault();

                    //Henter ut gameobjecetsa
                    List<GameObject> neighboursObject = new List<GameObject>();
                    foreach(var n in neigbors)
                    {
                        var no = cups.Where(c => c.name == n).FirstOrDefault();
                        neighboursObject.Add(no);
                    }
                    neighborTable.Add(cup, neighboursObject);
                }
            }
        }
       
        return neighborTable;
    }

    public static void CheckNeighbor(string currentRow, int colIndex, int rowIndex, List<string> list)
    {
        if (!IsOutOfBounds(currentRow, colIndex))
            if (currentRow[colIndex] == '1')
                list.Add(rowIndex + "-" + colIndex);
    }

    static bool IsOutOfBounds(string currentRow, int colIndex)
    {
        bool colOk = colIndex < 0 || colIndex >= currentRow.Length;
        if (colOk) return true;
        return false;
    }

}
