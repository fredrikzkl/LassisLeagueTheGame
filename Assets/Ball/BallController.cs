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

    //Blir satt true dersom denne ballen traff en kopp
    public bool didHitCup;

    //Traff rimmen til en kopp
    public bool didHitRim;

    //Blir satt til ja dersom denne ballen skal blir brukt til island
    public bool isIslandBall { get; set; }

    public float BallHitSoundFactor = 2f;

    //Sound
    float soundCooldown = 0f; //Brukes for å eliminere duplikate lyder når treffer objekter

    private void Awake()
    {
        isIslandBall = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        isInPlay = true;
        didHitCup = false;
        didHitRim = false;
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
            if (!didHitCup && isInPlay)
            {
                owner.GetComponent<PlayerRoundHandler>().stats.AddMiss(1);
            }
            isInPlay = false;
        }
    }

    private void FixedUpdate()
    {
        if (soundCooldown > 0f)
            soundCooldown -= 0.1f;
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

    public void SetAsHitBall()
    {
        didHitCup = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool soundCdOk = soundCooldown <= 0f;

        float ballVelocity = GetComponent<Rigidbody>().velocity.magnitude;

        float maxVolume = 8f;
        float minVolume = 0.8f;

        float volume = (ballVelocity - minVolume) / (maxVolume-minVolume);

        //float volume = (0.5f*(ballVelocity*2f)) / BallHitSoundFactor;
      
        if(collision.gameObject.tag == "Table")
        {
            PlayBallImpactSound("BallHitTable", volume);
        }
        if (collision.gameObject.tag == "Cup")
        {
            var cupYPos = collision.gameObject.transform.position.y;

            var rimLimit = cupYPos + ((cupYPos / 2) * 0.1);

            if (gameObject.transform.position.y > rimLimit)
            {
                didHitRim = true;
                PlayBallImpactSound("BallHitCupRim", volume);
            }
            else
            {
                PlayBallImpactSound("BallHitCupSide", volume);

            }
        }
    }

    public void PlayBallImpactSound(string soundeffect, float volume)
    {
        bool soundCdOk = soundCooldown <= 0f;
        if (soundCdOk)
        {
            FindObjectOfType<SoundManager>().PlaySound(soundeffect, volume);
            soundCooldown = 0.5f;
        }
    }

}
