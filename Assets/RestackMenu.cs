using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Formations;

public class RestackMenu : MonoBehaviour
{
    //Referanser
    public GameLogic game;
    public GameObject gameUiMenu;
    public RectTransform formationCardAnchorPoint;
    public GameObject formationCardPrefab;
    private Image panel;

    //OpponentsRack
    private PlayerRoundHandler opppnentRoundHandler;

    //FormationCards
    public List<GameObject> formationsCards;

    //Temps
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        panel = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    public void Initiate()
    {

        gameObject.SetActive(true);
        player = game.playerWithTheRound;
        //Setter fargene
        var pc = player.GetComponent<PlayerController>().playerColor;
        Color temp = new Color(pc.r, pc.g, pc.b, 0.1f);
        panel.color = temp;
        //Hvor mange restacks har denne spilleren?
        opppnentRoundHandler = game.GetOpponent(player).GetComponent<PlayerRoundHandler>();
        CreateFormationCards(opppnentRoundHandler.GetValidFormations());
    }

    void CreateFormationCards(List<Formation> formations)
    {
        var cardWidth = formationCardPrefab.GetComponent<RectTransform>().sizeDelta.x;
        var padding = 0.1f * cardWidth;

        var anchorPointX = formationCardAnchorPoint.position.x;
        //Om det er partall, flytter ankeret 0.5*bredde til venstre
        if (formations.Count % 2 == 0)
            anchorPointX -= ((0.5f * cardWidth) + padding);

        foreach (var f in formations)
        {
            //Lager nytt kort
            var newCard = Instantiate(formationCardPrefab, new Vector3(anchorPointX, formationCardAnchorPoint.position.y, 0), Quaternion.identity);
            formationsCards.Add(newCard);
            //Instansierer knappen
            Button buttonCtrl = newCard.GetComponent<Button>();
            buttonCtrl.onClick.AddListener(() => OnFormationCardClick(f.FormationString));
            //Setter kortet til å bli et barn av menyen
            newCard.transform.SetParent(gameObject.transform);
            //Setter tekst + bilde
            var formationVisualAnchor = player.tag == "player1" ? newCard.transform.GetChild(1) : newCard.transform.GetChild(2);
            var textOject = newCard.transform.GetChild(0);
            textOject.GetComponent<TextMeshProUGUI>().text = f.Name;
            

            //Setter x verdien til neste kort
            anchorPointX += (cardWidth + padding);
        }
    }

    private void OnFormationCardClick(string formationString)
    {
        if (player.GetComponent<PlayerRoundHandler>().restacks < 1)
        {
            Debug.Log(player.name + " does not have any restacks remaining");
            return;
        }
        GameObject opponent = game.GetOpponent(player);
        ResumeGame();
        opponent.GetComponent<PlayerRoundHandler>().Restack(formationString);
        player.GetComponent<PlayerRoundHandler>().restacks--;
    }


    public void ResumeGame()
    {
        RemoveFormationCards();
        Time.timeScale = 1f;
        GameManager.gameIsPaused = false;
        gameObject.SetActive(false);
    }

    private void RemoveFormationCards()
    {
        foreach(var c in formationsCards)
        {
            Destroy(c);
        }
        formationsCards.Clear();
    }
}
