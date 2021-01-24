using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    
    public Material rimMaterial;

    private Material standardMaterial;
    private Material isHitMaterial;

    public GameObject Rack { get; set; }

    bool isHit;
    //Spilleren som traff koppen
    public GameObject hittingPlayer { get; set; }
    public bool isBitchCup = false;
    

    //IslandCup: Settes true dersom spiller invoker island
    bool blinking;
    public bool isIslandCup;
    bool successfullyHitIsland;
  

    //Reracking stuff
    public Vector3 newPosition;


    private void Start()
    {
        isHit = false;
        isIslandCup = false;
        blinking = false;
        successfullyHitIsland = false;
        newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void Update()
    {
        float step = 2.5f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

        if (blinking)
        {
            float t = Mathf.Sin(Time.fixedTime * 6.5f);
            if (t > 0)
                SetMainMaterial(standardMaterial);
            else
                SetMainMaterial(isHitMaterial);
        }
    }



    //Når 
    public void SetIsland(bool status)
    {
        isIslandCup = status;
        blinking = status;
        if(status == false)
        {
            SetMainMaterial(standardMaterial);
        }
    }

    public void SetMaterials(Material standard, Material hit)
    {
        isHitMaterial = hit;
        standardMaterial = standard;
        SetMainMaterial(standardMaterial);
    }

    private void SetMainMaterial(Material material)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        var m = new Material[2];
        m[0] = rimMaterial;
        m[1] = material;
        renderer.materials = m;
    }

    internal void SetAsBitchCup()
    {
        isBitchCup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ballen traff koppen!!
        if(other.tag == "Ball")
        {
            //Nøytraliserer ballen
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<BallController>().BallOutOfBounds(1f);

            //Check for win condition
            Rack.GetComponent<CupRack>().CheckIfLost(gameObject);

            blinking = false;
            //Sjekker om det var island
            if(other.GetComponent<BallController>().isIslandBall)
            {
                if (isIslandCup)
                {
                    Debug.Log("DUDE ISLAND!!");
                    FindObjectOfType<SoundManager>().PlaySound("CrowdCheer");
                    successfullyHitIsland = true;

                }
                else
                {
                    Debug.Log("Traff en kopp som ikke er island koppen");
                    FindObjectOfType<SoundManager>().PlaySound("CrowdGroan");
                    return;
                }
            }

            CheckBitchCup();

            FindObjectOfType<SoundManager>().PlaySound("CupHit");

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<BallController>().BallOutOfBounds(1f);

            hittingPlayer = other.GetComponent<BallController>().owner;
            
            if (hittingPlayer == gameObject.transform.root.gameObject)
            {
                FindObjectOfType<SoundManager>().PlaySound("CrowdGroan");
            }
            

            UpdateToHitMaterial();
            RegisterHit(other.gameObject);
        }
    }

    private void CheckBitchCup()
    {
        //Bitchcup skjer om du treffer bitchcup som første kopp, som ikke er eye to eye
        if (isBitchCup && FindObjectOfType<GameLogic>().gameStage == GameStage.Game)
        {
            var startFormation = Formations.GetStartFormation(FindObjectOfType<GameRules>().StartFormation);
            int originalCupCount = Formations.GetCupCountFromFormation(startFormation);

            if(Rack.GetComponent<CupRack>().GetCupList().Count == originalCupCount)
            {
                FindObjectOfType<Announcer>().BitchCup();
            }
        }
    }

    public void RegisterHit(GameObject ball)
    {
        GameObject owner = ball.GetComponent<BallController>().owner;
        owner.GetComponent<PlayerRoundHandler>().HitCups.Add(gameObject);
        ball.GetComponent<BallController>().SetAsHitBall();
        owner.GetComponent<PlayerRoundHandler>().stats.AddHit(1);
    }

    private void UpdateToHitMaterial()
    {
        if(isHitMaterial != null)
        {
            SetMainMaterial(isHitMaterial);
        }
    }

    public void UpdateToStandardMaterial() {
        SetMainMaterial(standardMaterial);
    }

    public bool isSuccessfullyHitIsland()
    {
        return successfullyHitIsland;
    }
}
