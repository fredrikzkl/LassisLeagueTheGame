using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject owner { get; set; }
    //Hjelpevariabler

    private float timingHitOutOfBounds; //Legacy 
    
    //Hvor mange sekunder ballen skal være "aktiv", altså i spill
    public float timeAlive = 4f; 

    //Dersom on isInPlay, så 
    public bool isInPlay;


    // Start is called before the first frame update
    void Start()
    {
        isInPlay = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(timeAlive > 0)
        {
            timeAlive = timeAlive - Time.smoothDeltaTime;
        }
        else
        {
            isInPlay = false;
        }
    }

    public void BallOutOfBounds(float removeAfterSeconds)
    {
        timeAlive = removeAfterSeconds;
    }

   
   
}
