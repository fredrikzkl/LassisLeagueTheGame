using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourTable
{

    public List<(GameObject, List<GameObject>)> table;

    public NeighbourTable()
    {
        table = new List<(GameObject, List<GameObject>)>();
    }

    public List<GameObject> GetNeighbours(GameObject cup)
    {
        foreach (var e in table)
        {
            if (e.Item1 == cup)
            {
                return e.Item2;
            }
        }
        return null;
    }

    public int GetNeighbourCount(GameObject cup)
    {
        foreach(var e  in table)
        {
            if (e.Item1 == cup)
                return e.Item2.Count;
        }
        return 0;
    }


    public void Add(GameObject cup, List<GameObject> neighbours)
    {
        table.Add((cup,neighbours));
    }

    public void Remove(GameObject key)
    {
        for(int i = 0; i < table.Count; i++)
        {
            var tempEntry = table[i];
            if (table[i].Item1 == key)
            {
                table.Remove(tempEntry);
            }
        }
    }

    public void RemoveNeighbour(GameObject target, GameObject neighbour)
    {
        foreach (var e in table)
        {
            if (e.Item1 == target)
            {
                e.Item2.Remove(neighbour);
            }
        }
    }

}
