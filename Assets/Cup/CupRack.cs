using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Formations;

public class CupRack : MonoBehaviour
{

    public GameObject cupPrefab;
    public Material standard;
    public Material hit;

    private Transform location;
    public int direction = -1;

    List<GameObject> cupList;

    NeighbourTable neighbourTable;

    string currentFormation;


    //Hjelpevariabler
    float diameter;

    private void Start()
    {
        location = GetComponent<Transform>();
        cupList = new List<GameObject>();

        if (location == null)
        {
            Debug.Log("Rack doesn not have a location");
            return;
        }

        diameter = cupPrefab.GetComponent<Renderer>().bounds.size.x;
        CreateRack(FindObjectOfType<GameRules>().StartFormation);
    }

    private void CreateRack(StartFormation formation)
    {
        string f = GetStartFormation(formation);
        var pm = CreatePositionMatrix(f, location.position, direction, diameter);
        for (int i = 0; i < pm.Item1.Count; i++)
        {
            SpawnCup(pm.Item1[i], pm.Item2[i]);
        }

        neighbourTable = DetermineCupNeighbors(cupList, f);


        currentFormation = f;
    }


    public GameObject SpawnCup(Vector3 pos, string id)
    {
        var c = Instantiate(cupPrefab, pos, cupPrefab.transform.rotation);
        c.GetComponent<CupController>().SetMaterials(standard, hit);
        c.GetComponent<CupController>().Rack = gameObject;
        cupList.Add(c);
        c.name = id;
        return c;
    }

    public GameObject GetCupFromName(string name)
    {
        foreach (var cup in cupList)
        {
            if (cup.name == name)
                return cup;
        }
        Debug.Log("Cup " + name + " could not be found");
        return null;
    }


    public void Rerack(string formation)
    {
        var pm = CreatePositionMatrix(formation, location.position, direction, diameter);
        var positionsInRerack = pm.Item1;

        if (positionsInRerack.Count != cupList.Count)
        {
            Debug.Log("Error: Can't rerack! Formation doesnt match with number of cups");
            return;
        }
        for (int i = 0; i < cupList.Count; i++)
        {
            //Oppdaterer posisjon
            cupList[i].GetComponent<CupController>().newPosition = positionsInRerack[i];
            //Oppdaterer navn
            cupList[i].gameObject.name = pm.Item2[i];
        }
        //Henter nye nabotabellen
        var newNeighbourTable = DetermineCupNeighbors(cupList, formation);
        neighbourTable = newNeighbourTable;
        currentFormation = formation;
    }

    public void RemoveFromCupList(GameObject cup)
    {
        cupList.Remove(cup);
    }

    public int GetCupCount()
    {
        return cupList.Count;
    }

    public List<GameObject> GetCupList()
    {
        return cupList;
    }

    //TODO: Se igjennom denne:
    //Brukt hvis begge blir ballene blir truffet i samme kopp
    public List<GameObject> PickRandomCups(List<GameObject> exlcude, int removeCount)
    {
        
        if (removeCount + 1 >= cupList.Count)
            return cupList;
        List<GameObject> tempCups = new List<GameObject>();
        var rand = new System.Random();
        while (tempCups.Count != removeCount)
        {
            var picked = cupList[rand.Next(0, cupList.Count - 1)];
            if (tempCups.Contains(picked) || exlcude.Contains(picked))
                continue;
            tempCups.Add(picked);
        }
        return tempCups;
    }

    public void UpdateNeighborTable(GameObject cup)
    {
        var neighboursToHitCup = neighbourTable.GetNeighbours(cup);
        if (neighboursToHitCup == null) return;
        foreach (var n in neighboursToHitCup)
        {
            neighbourTable.RemoveNeighbour(n, cup);
        }
        neighbourTable.Remove(cup);
    }

    public List<GameObject> CheckForIsland()
    {
        List<GameObject> islandCups = new List<GameObject>();
        if (cupList.Count < 3) return islandCups;

        foreach (var cup in cupList)
        {
            var neighbours = neighbourTable.GetNeighbours(cup);
            if (neighbours == null) return islandCups;

            if (neighbours.Count == 0)
            {
                islandCups.Add(cup);
            }
        }
        if (islandCups.Count == 0) return islandCups;
        //Hvis det er en som står alene, må vi også sjekke at det er et fastland,
        //altså at det er en annen kopp som har en nabo
        bool mainLandExists = false;
        foreach(var cup in cupList)
        {
            if (islandCups.Contains(cup)) continue;
            if(neighbourTable.GetNeighbours(cup).Count > 0)
            {
                mainLandExists = true;
                break;
            }
        }

        if (mainLandExists == false)
            islandCups.Clear();

        return islandCups;
    }



}
