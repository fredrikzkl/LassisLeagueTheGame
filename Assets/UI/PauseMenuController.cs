using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public CanvasGroup HUD;

    private void Awake()
    {
        if(HUD == null)
        {
            Debug.LogWarning("PauseMenuController is missing HUD referance");
        }
    }

    public void PauseGame()
    {
        HUD.alpha = 0f;
        FindObjectOfType<SoundManager>().PlaySound("Click");
        FindObjectOfType<Music>().MuffleSound();

        gameObject.SetActive(true);
        GameManager.Pause();
    }

    public void ResumeGame()
    {
        HUD.alpha = 1f;
        FindObjectOfType<SoundManager>().PlaySound("Click");
        FindObjectOfType<Music>().UnMuffleSound();
        gameObject.SetActive(false);
        GameManager.Resume();
    }

    public void QuitGame()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        GameManager.Resume();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
