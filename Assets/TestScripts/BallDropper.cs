using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public GameObject ball;
    //Laget for å genere baller i lufta, for testing av kopptreff
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnCup();
        }        
    }

    void SpawnCup()
    {
        var b = Instantiate(ball, gameObject.transform.position, Quaternion.identity);
    }
}
