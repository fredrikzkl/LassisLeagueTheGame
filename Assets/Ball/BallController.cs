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

    private void OnCollisionEnter(Collision collision)
    {
        bool soundCdOk = soundCooldown <= 0f;
        float ballVelocity = GetComponent<Rigidbody>().velocity.magnitude;
        float volume = 0.5f*(ballVelocity*2f);
        
        if(collision.gameObject.tag == "Table")
        {
            PlayBallImpactSound("BallHitTable", ballVelocity);
        }
        if(volume > 1f)
        {
            if (collision.gameObject.tag == "Cup")
            {
                var cupYPos = collision.gameObject.transform.position.y;

                var rimLimit = cupYPos + ((cupYPos / 2) * 0.1);

                if (gameObject.transform.position.y > rimLimit)
                {
                    PlayBallImpactSound("BallHitCupRim", ballVelocity);
                }
                else
                {
                    PlayBallImpactSound("BallHitCupSide", ballVelocity);
                    
                }
            }
        }
    }

    public void PlayBallImpactSound(string soundeffect, float volume)
    {
        bool soundCdOk = soundCooldown <= 0f;
        if (soundCdOk)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(soundeffect, volume);
            soundCooldown = 0.5f;
        }
    }

}
