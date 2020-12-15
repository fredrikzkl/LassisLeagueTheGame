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

    //Blir satt til ja dersom denne ballen skal blir brukt til island
    public bool isIslandBall { get; set; }

    private void Awake()
    {
        isIslandBall = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        isInPlay = true;
    }

    private void OnDestroy()
    {
        //Dersom denne ballen dør og skal blir brukt til islandkoppen, fjernes alle islandkopper
        if (isIslandBall)
        {
            var opponentRack = FindObjectOfType<GameLogic>().GetOpponentCupRack(owner);
            opponentRack.DisableIsland();
        }
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

    public void SetAsIslandBall()
    {
        GetComponent<TrailRenderer>().enabled = true;
        isIslandBall = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Table")
        {
            Debug.Log("HIT WITH TABLE");
            FindObjectOfType<SoundManager>().PlaySoundEffect("BallHitTable");
        }
    }

}
