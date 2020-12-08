using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestackMenu : MonoBehaviour
{
    //Referanser
    public GameLogic game;
    public GameObject gameUiMenu;
    private Image panel;

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
        var pc = player.GetComponent<PlayerController>().playerColor;
        Color temp = new Color(pc.r, pc.g, pc.b, 0.1f);
        panel.color = temp;
    }

    public void RestackButtonOnClick()
    {
        if (player.GetComponent<PlayerRoundHandler>().restacks < 1)
        {
            Debug.Log(player.name + " does not have any restacks remaining");
            return;
        }
        GameObject opponent = game.GetOpponent(player);
        ResumeGame();
        opponent.GetComponent<PlayerRoundHandler>().Restack();
        player.GetComponent<PlayerRoundHandler>().restacks--;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameManager.gameIsPaused = false;
        gameObject.SetActive(false);
      


    }

    

}
