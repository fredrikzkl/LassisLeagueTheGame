﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupRack : MonoBehaviour
{

    public GameObject cupPrefab;
    public Material standard;
    public Material hit;

    private Transform location;
    public int direction = -1;

    List<GameObject> cupList;


    //Hjelpevariabler
    float diameter;
    //public float tightFactor = 0.865f;
    public float tightFactor = 0.7f;

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
        
        CreateRack(Formations.HalfHouse);
    }

    

    private void CreateRack(string formation)
    {
        foreach(var pos in CreatePositionMatrix(formation))
        {
            SpawnCup(pos);
        }
    }


    public void SpawnCup(Vector3 pos) {
        var c = Instantiate(cupPrefab, pos, cupPrefab.transform.rotation);
        c.GetComponent<CupController>().SetMaterials(standard, hit);
        c.GetComponent<CupController>().Rack = gameObject;
        cupList.Add(c);
    }

    //Lager en liste over alle posisjonene koppene skal stå
    public List<Vector3> CreatePositionMatrix(string formation)
    {
        List<Vector3> positionMatrix = new List<Vector3>();
        Vector3 tempPos = new Vector3(location.position.x, location.position.y, location.position.z);
        tempPos.z -= (2 * diameter);
        //Decoding string:
        string[] rows = formation.Split('#');
        foreach (var row in rows)
        {
            var rowResult = CreatePositionMatrixRow(tempPos, row);
            foreach(var position in rowResult)
            {
                positionMatrix.Add(position);
            }
            tempPos.x += (direction * diameter * tightFactor);
        }
        return positionMatrix;

    }

    public List<Vector3> CreatePositionMatrixRow(Vector3 pos, string row)
    {
        List<Vector3> rowList = new List<Vector3>();
        foreach (var symbol in row)
        {
            if (symbol == '1')
            {
                rowList.Add(pos);
            }
            pos.z = pos.z + (0.5f * diameter);
        }
        return rowList;
    }


    public void Rerack(string formation)
    {
        var positionsInRerack = CreatePositionMatrix(formation);
        if(positionsInRerack.Count != cupList.Count)
        {
            Debug.Log("Error: Can't rerack! Formation doesnt match with number of cups");
            return;
        }
        for(int i = 0; i < cupList.Count; i++)
        {
            cupList[i].GetComponent<CupController>().newPosition = positionsInRerack[i];
        }
    }

    public void RemoveFromCupList(GameObject cup)
    {
        cupList.Remove(cup);
      
        if(cupList.Count == 2)
        {
            Rerack(Formations.Gentlemans);
        }
    }

    public int GetCupCount()
    {
        return cupList.Count;
    }

    
}
