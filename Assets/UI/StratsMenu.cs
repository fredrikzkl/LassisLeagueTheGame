using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Formations;

public class StratsMenu : MonoBehaviour
{
    //Referanser
    public GameLogic game;
    //Baren nederest under gameplay (slider, ballene etc)
    public GameObject lowerUiMenu;

    public RectTransform formationCardAnchorPoint;
    public RectTransform islandCardAnchorPoint;
    public GameObject formationCardPrefab;
    public GameObject cupImagePrefab;
    private Image panel;

    public GameObject IslandSection;
    public GameObject RestackSecion;

    //OpponentsRack
    private PlayerRoundHandler opppnentRoundHandler;

    //FormationCards
    public List<GameObject> stratCards;


    //Temps
    GameObject player;
    int restacks;
    int islands;

    //Hjelpevariabler
    private string currentFormation;
    private float cardWidth;
    private float padding;
    private float cupSpriteDiameter;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        panel = transform.GetChild(0).gameObject.GetComponent<Image>();
        //Setter verdier brukt for stratscards
        cardWidth = formationCardPrefab.GetComponent<RectTransform>().sizeDelta.x;
        padding = 0.1f * cardWidth;
        cupSpriteDiameter = cupImagePrefab.GetComponent<RectTransform>().sizeDelta.x;
    }


    public void Initiate()
    {
        gameObject.SetActive(true);
        SetLowerUiActive(false);
        player = game.playerWithTheRound;
        //Setter fargene
        var playerController = player.GetComponent<PlayerController>();
        var pc = playerController.playerColor;
        Color temp = new Color(pc.r, pc.g, pc.b, 0.1f);
        panel.color = temp;
        //Henter ut egen stats
        var playerRoundHandler = player.GetComponent<PlayerRoundHandler>();
        islands = playerRoundHandler.islands;
        restacks = playerRoundHandler.restacks;
        //Henter ut racken til motspilleren
        opppnentRoundHandler = game.GetOpponent(player).GetComponent<PlayerRoundHandler>();
        //Sjekker reracks
        List<Formation> validFormations = opppnentRoundHandler.GetValidFormations();
        if (validFormations.Count > 0 && restacks > 0)
        {
            CreateFormationCards(opppnentRoundHandler.GetValidFormations());
            RestackSecion.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Restack  (" + restacks + ")";
            RestackSecion.active = true;
        }

        //Henter ut islandkoppene
        if (playerController.islandCups.Count > 0 && islands > 0)
        {
            currentFormation = opppnentRoundHandler.cupRack.GetComponent<CupRack>().currentFormation;
            CreateIslandCards(playerController.islandCups);
            IslandSection.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Island  (" + islands + ")";
            IslandSection.active = true;
        }
        else
        {
            
        }
        //Formations
        
        
    }


    void CreateFormationCards(List<Formation> formations)
    {
        var anchorPointX = AdujustAnchorpointX(formationCardAnchorPoint.position.x, formations.Count);
       
        foreach (var f in formations)
        {
            var newCard = AddNewStratsCard(new Vector3(anchorPointX, formationCardAnchorPoint.position.y, 0), f.Name, formationCardAnchorPoint.transform);

            //Instansierer knappen
            newCard.GetComponent<Button>().onClick.AddListener(() => OnFormationCardClick(f.FormationString));
            //Setter kortet til å bli et barn av menyen

            //Henter ut verdier fra spilleren
            var correctedVisualAnchor = GetCorrectCupVisualAnchor(newCard);
            Transform formationVisualAnchor = correctedVisualAnchor.Item1;
            int direction = correctedVisualAnchor.Item2;
         
            //Lager formasjonen
            List<Vector3> positions = Create2DPositionMatrix(f.FormationString, formationVisualAnchor.transform.position, direction, cupSpriteDiameter).Item1;
            foreach (var p in positions)
            {
                var cupImage = Instantiate(cupImagePrefab, p, Quaternion.identity);
                cupImage.transform.SetParent(newCard.transform);
                Color color = opppnentRoundHandler.gameObject.GetComponent<PlayerController>().playerColor;
                cupImage.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.9f);
            }
            //Setter x verdien til neste kort
            anchorPointX += (cardWidth + padding);
        }
    }

    (Transform, int) GetCorrectCupVisualAnchor(GameObject card)
    {
        Transform cupVisualsAnchor;
        int direction;
        if (player.gameObject.tag.Equals("Player1"))
        {
            cupVisualsAnchor = card.transform.GetChild(2);
            direction = -1;
        }
        else
        {
            cupVisualsAnchor = card.transform.GetChild(1);
            direction = 1;
        }
        return (cupVisualsAnchor, direction);
    }


    void CreateIslandCards(List<GameObject> islandCups)
    {
        var anchorPointX = AdujustAnchorpointX(islandCardAnchorPoint.position.x, islandCups.Count);

        for (int c = 0; c < islandCups.Count; c++)
        {
            string cardName = "Island";
            if (islandCups.Count > 1)
                cardName += "#" + (c + 1);
            //Lager kortet
            var newIslandCard = AddNewStratsCard(new Vector3(anchorPointX, islandCardAnchorPoint.transform.position.y, 0), cardName, islandCardAnchorPoint.transform);

            var cup = islandCups[c];
            newIslandCard.GetComponent<Button>().onClick.AddListener(() => OnIslandCardClick(cup));
            //Visuals
            var correctedVisualAnchor = GetCorrectCupVisualAnchor(newIslandCard);
            Transform formationVisualAnchor = correctedVisualAnchor.Item1;
            int direction = correctedVisualAnchor.Item2;

            var pm = Create2DPositionMatrix(currentFormation, formationVisualAnchor.transform.position, direction, cupSpriteDiameter);

            var opponentRack = opppnentRoundHandler.cupRack.GetComponent<CupRack>();
            for (int i = 0; i<pm.Item2.Count; i++)
            {
                var tempCumpName = pm.Item2[i];
                foreach(var cupInRack in opponentRack.GetCupList())
                {
                    if (tempCumpName.Equals(cupInRack.name))
                    {
                        var cupImage = Instantiate(cupImagePrefab, pm.Item1[i], Quaternion.identity);
                        cupImage.transform.SetParent(newIslandCard.transform);
                        if (cupInRack == islandCups[c])
                        {
                            cupImage.GetComponent<Image>().color = new Color(255, 215, 0, 0.9f);
                        }
                        else
                        {
                            Color color = opppnentRoundHandler.gameObject.GetComponent<PlayerController>().playerColor;
                            cupImage.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.9f);
                        }
                    }                    
                }
            }
       
            //Setter x verdien til neste kort
            anchorPointX += (cardWidth + padding);
        }
    }

    GameObject AddNewStratsCard(Vector3 position , string name, Transform parent)
    {
        var newCard = Instantiate(formationCardPrefab, position, Quaternion.identity);
        stratCards.Add(newCard);
        //Navn på gameobjectet
        newCard.name = name;
        newCard.transform.SetParent(parent);
        //Setter tekst + bilde 
        newCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        return newCard;
    }


    float AdujustAnchorpointX(float origin, int count)
    {
        if (count % 2 == 0)
            origin -= ((0.5f * cardWidth));
        if (count > 1)
            origin -= ((count - 1) / 2 * cardWidth + padding);
        return origin;
    }

    void OnIslandCardClick(GameObject cup)
    {
        if (player.GetComponent<PlayerRoundHandler>().islands < 1)
        {
            Debug.Log(player.name + " does not have any islands remaining");
            return;
        }
        ResumeGame();

        GameObject opponent = game.GetOpponent(player);
        opponent.GetComponent<PlayerRoundHandler>().InvokeIsland(cup);
        player.GetComponent<PlayerRoundHandler>().islands--;
    }

    private void OnFormationCardClick(string formationString)
    {
        if (player.GetComponent<PlayerRoundHandler>().restacks < 1)
        {
            Debug.Log(player.name + " does not have any restacks remaining");
            return;
        }
        ResumeGame();

        GameObject opponent = game.GetOpponent(player);
        opponent.GetComponent<PlayerRoundHandler>().Restack(formationString);
        player.GetComponent<PlayerRoundHandler>().restacks--;
    }


    public void ResumeGame()
    {
        RemoveFormationCards();
        Time.timeScale = 1f;
        GameManager.gameIsPaused = false;
        IslandSection.active = false;
        RestackSecion.active = false;
        gameObject.SetActive(false);
        SetLowerUiActive(true);


    }

    private void RemoveFormationCards()
    {
        foreach(var c in stratCards)
        {
            Destroy(c);
        }
        stratCards.Clear();
    }

    void SetLowerUiActive(bool active)
    {
        for (int i = 0; i < lowerUiMenu.transform.childCount; i++)
        {
            lowerUiMenu.transform.GetChild(i).gameObject.SetActive(active);
        }
    }

}
