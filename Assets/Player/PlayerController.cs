using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerType
{
    Player, AI
}


public class PlayerController : MonoBehaviour
{
    
    public PlayerType playerType;

    public AICore AI { get; set; }
    public string Name {get;set;}

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


    //Power
    private bool isChargingUp;
    private float currentPower;
    //Brukt for 2D
    //private float chargeUpRate = 2.5f;

    public bool forgivingPowerbar;
    private int chargeUpRateDirection = 1;
    private float chargeUpRate = 0.215f;
 
    //Throwing helpvariables
    float currentAngle;

    //Island
    public bool nextThrowIsIsland { get; set; }
    public bool isBallsBackRound;


    private void Start()
    {
        roundHandler = GetComponent<PlayerRoundHandler>();
        isChargingUp = false;
        currentPower = 0f;
        powerSlider.value = 0;
        aimArrow.GetComponent<MeshRenderer>().enabled = false;
        hud.DeactivateStratsButton();
        nextThrowIsIsland = false;
        isBallsBackRound = false;
        //AI 
        if(IsAI())
        {
            AI = GetComponent<AICore>();
            if (AI == null)
                Debug.LogWarning("Player " + gameObject + " is missing an AI Core component");
            name = AI.difficulty.ToString() + " AI";
        }
        else
        {
            Name = gameObject.name;
        }
            
    }

    public void BallsBack()
    {
        Debug.Log(gameObject.name + " gets the balls back");
        throwsRemaining = FindObjectOfType<GameRules>().BallsBackCount;
        hud.UpdateBallIndicator(throwsRemaining);
        isBallsBackRound = true;
        if(!IsAI())
            aimArrow.GetComponent<AimArrowController>().Enable();
    }

    public void StartRound(int throws)
    {

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
            bool islandInvoked = false;
            //Har vi island?
            islandCups = opponentCupRack.CheckForIsland();
            if (islandCups.Count > 0 && GetComponent<PlayerRoundHandler>().islands > 0)
            {
                if (IsAI() && AI.DecidesIsland(islandCups))
                    islandInvoked = true;

                hasStrats = true;
            }
            //Kan vi restacke?
            if (roundHandler.restacks > 0 && Formations.GetValidFormations(opponentCupRack.GetCupCount()).Count > 0 && !islandInvoked)
            {
                hasStrats = true;
                if (IsAI())
                    AI.DecideRestack();
            }
            //Om vi har noen syke strats, så viser vi knappen
            if (hasStrats && !IsAI()) 
            {
                hud.ActivateStratsButton();
            }
        }
        //Legger til baller
        throwsRemaining = throws;
        hud.UpdateColors(playerColor, throwsRemaining);
        isBallsBackRound = false;
        //Starter pilen
        if (!IsAI())
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
            if (forgivingPowerbar)
            {
                if (currentPower < 0 && chargeUpRateDirection == -1)
                {
                    chargeUpRateDirection = 1;
                }
                if (currentPower > 10 && chargeUpRateDirection == 1)
                {
                    chargeUpRateDirection = -1;
                }

                currentPower += chargeUpRate * chargeUpRateDirection;
            }
            else
            {
                if (currentPower < 10)
                    currentPower += chargeUpRate;
            }
            
            powerSlider.value = currentPower;

            return;
        }
    }

    void Update()
    {
        var gameLogic = FindObjectOfType<GameLogic>();
        if (gameLogic.playerWithTheRound == gameObject && gameLogic.gameStage != GameStage.GameOver)
        {
            if (throwsRemaining > 0)
            {
                if(IsAI())
                {
                    if (AI.IsReadyToThrow())
                    {
                        var targetCup = AI.PickACup();
                        var shot = AI.CalculateShot(playerHand, targetCup.transform, GetPlayerDirection());
                        ThrowBall(shot);
                        throwsRemaining -= 1;
                        hud.UpdateBallIndicator(throwsRemaining);
                    }
                    return;
                }

                if (isChargingUp)
                {
                    //Slipper opp
                    if ((Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Space)) && !GameManager.gameIsPaused)
                    {
                        ThrowBall();
                        //Gamme updates
                        isChargingUp = false;
                        currentPower = 0;
                        throwsRemaining -= 1;
                        //UI
                        aimArrow.GetComponent<AimArrowController>().StartRotating();
                        powerSlider.value = 0;
                        hud.UpdateBallIndicator(throwsRemaining);
                        //Replay
                        //FindObjectOfType<GameLogic>().AddThrowToReplay(temp_arc, temp_angle, temp_power, playerHand);
                    }
                }

                //Første klikk
                if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && !GameManager.gameIsPaused && !IsPointerOverUIObject())
                {
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

    void ThrowBall()
    {
        //var xyAngle = aimArrow.GetComponent<AimArrowController>().RelativeXYAngle();
        var xyAngle = aimArrow.GetComponent<AimArrowController>().GetRelativeXYAngle();
        var xzAngle = currentAngle;
        var power = currentPower;
        ThrowBall(xyAngle, xzAngle, power);
    }

    void ThrowBall(ThrowData shot)
    {
        ThrowBall(shot.XYAngle, shot.XZAngle, shot.Power);
    }

    public void ThrowBall(float xyAngle, float xzAngle, float power)
    {
        var ball = Instantiate(ballPrefab, playerHand.position, Quaternion.identity);
        ball.name = "ball_" + throwsRemaining;
        ball.GetComponent<BallController>().owner = gameObject;
        roundHandler.ThrownBalls.Add(ball);
        if (nextThrowIsIsland)
        {
            ball.GetComponent<BallController>().SetAsIslandBall();
            nextThrowIsIsland = false;
        }

        if (isBallsBackRound)
        {
            ball.GetComponent<TrailRenderer>().enabled = true;
        }

        Vector3 intialValues = GetComponent<PlayerThrowLogic>().SphericalCoordinates(xyAngle, xzAngle, power);

        ball.GetComponent<Rigidbody>().velocity = intialValues;

        Debug.Log("Final XY: [[" + xyAngle + "]]" + " -- X: " + intialValues.x + " Y:" + intialValues.y + " Z:" + intialValues.z);

    }

    public int GetPlayerDirection()
    {
        return aimArrow.GetComponent<AimArrowController>().direction;
    }

    public void Disable()
    {
        aimArrow.active = false;
    }

    public bool IsAI()
    {
        return playerType == PlayerType.AI;
    }

   
}
