using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //Player utilites
    public Transform playerHand;
    public GameObject ballPrefab;
    public GameObject aimArrow;

    //Scripts
    public PlayerRoundHandler roundHandler;

    //Gamestuff
    public Color playerColor;
    public Slider powerSlider;
    public GameHUD hud;
    
    private int throwsRemaining = 0;
    public List<GameObject> islandCups { get; set; }

    //Arc
    private Vector3 mousePosition;
    public float maxMouseDistance = 15f;

    //Power
    public float powerConstant = 0.01f;
    private bool isChargingUp;
    private float currentPower;

    //Throwing helpvariables
    float currentAngle;

    private void Start()
    {
        isChargingUp = false;
        currentPower = 0f;
        powerSlider.value = 0;
        aimArrow.GetComponent<MeshRenderer>().enabled = false;
        hud.DeactivateStratsButton();
        roundHandler = GetComponent<PlayerRoundHandler>();
    }

    public void BallsBack()
    {
        Debug.Log(gameObject.name + " gets the balls back");
        throwsRemaining = FindObjectOfType<GameRules>().BallsBackCount;
        hud.UpdateBallIndicator(throwsRemaining);
        aimArrow.GetComponent<AimArrowController>().Enable();
    }

    public void StartRound()
    {
        Debug.Log(gameObject.name + "'s round!");

        CupRack opponentCupRack = FindObjectOfType<GameLogic>().GetOpponentCupRack(gameObject);


        //Har vi genltemans?
        if (opponentCupRack.GetCupCount() == 2)
        {
            opponentCupRack.Rerack(Formations.Gentlemans.FormationString);
        }
        else
        {
            //Har vi strats?
            bool hasStrats = false;
            //Har vi island?
            islandCups = opponentCupRack.CheckForIsland();
            if (islandCups.Count > 0)
            {
                Debug.Log(gameObject.name + " has island with " + islandCups.Count + " cups!");
                islandCups.ForEach(ic => Debug.Log(ic.name));
                hasStrats = true;
            }
            //Kan vi restacke?
            if (roundHandler.restacks > 0 && Formations.GetValidFormations(opponentCupRack.GetCupCount()).Count > 0)
            {
                hasStrats = true;
            }
            //Om vi har noen syke strats, så viser vi knappen
            if (hasStrats)
            {
                hud.ActivateStratsButton();
            }
        }
        //Legger til baller
        throwsRemaining = FindObjectOfType<GameRules>().NumberOfBallsPrRound;
        hud.UpdateColors(playerColor, throwsRemaining);
        //Starter pilen
        aimArrow.GetComponent<AimArrowController>().Enable();
    }



    public void EndRound()
    {
        aimArrow.GetComponent<AimArrowController>().Disable();
        //Fjerner meny knappen. Slår det på igjen dersom vi trenger den
        hud.DeactivateStratsButton();
        roundHandler.EndRound();
    }


    private void FixedUpdate()
    {
        if (isChargingUp)
        {
            if (currentPower < 100f)
            {
                currentPower += 2.5f;
                powerSlider.value = currentPower;
            }
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (FindObjectOfType<GameLogic>().playerWithTheRound == gameObject)
        {
            if (throwsRemaining > 0)
            {
                if (isChargingUp)
                {
                    //Slipper opp
                    if ((Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Space)) && !GameManager.gameIsPaused)
                    {
                        Vector2 temp_arc = GetAimingResults();
                        float temp_angle = currentAngle;
                        float temp_power = currentPower;
                        ThrowBall(temp_arc, temp_angle, temp_power);
                        //Gamme updates
                        isChargingUp = false;
                        currentPower = 0;
                        throwsRemaining -= 1;
                        mousePosition = Vector3.zero;
                        //UI
                        aimArrow.GetComponent<AimArrowController>().StartRotating();
                        powerSlider.value = 0;
                        hud.UpdateBallIndicator(throwsRemaining);
                        //Replay
                        FindObjectOfType<GameLogic>().AddThrowToReplay(temp_arc, temp_angle, temp_power, playerHand);
                    }
                }

                //Første klikk
                if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && !GameManager.gameIsPaused && !IsPointerOverUIObject())
                {
                    mousePosition = Input.mousePosition;
                    currentAngle = aimArrow.GetComponent<AimArrowController>().StopRotating();
                    isChargingUp = true;
                    //Fjerner muligheten din for å gjøre valg
                    hud.DeactivateStratsButton();
                }
            }
            else
            {
                EndRound();
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    public void ThrowBall(Vector2 arc, float angle, float power)
    {
        var ball = Instantiate(ballPrefab, playerHand.position, Quaternion.identity);
        ball.name = "ball_" + throwsRemaining;
        ball.GetComponent<BallController>().owner = gameObject;
        roundHandler.ThrownBalls.Add(ball);



        /*
        var adjustedPower = power * 0.045f;
        float x = arc.x * adjustedPower;
        float y = arc.y * adjustedPower;

        ball.GetComponent<Rigidbody>().velocity = new Vector3(x,y, angle*2f);
        */

        //Debug.Log(angle);
        var adjustedPower = power * 0.075f;

        float y = adjustedPower;

        float x = Mathf.Cos(angle) * adjustedPower;
        float z = Mathf.Sin(angle) * adjustedPower;

        ball.GetComponent<Rigidbody>().velocity = new Vector3(x, y, z);


    }

    private float LimitDeltaValues(float deltaValue)
    {
        if (deltaValue > maxMouseDistance) return maxMouseDistance;
        else if (deltaValue < -maxMouseDistance) return -maxMouseDistance;
        return deltaValue;
    }

    //Lecay
    private Vector2 GetAimingResults()
    {
        var finalMousePos = Input.mousePosition;
        Vector2 aim = new Vector2();
        aim.x = -LimitDeltaValues(mousePosition.x - finalMousePos.x) * powerConstant;
        aim.y = LimitDeltaValues(mousePosition.y - finalMousePos.y) * powerConstant;

        return aim;
    }


}
