using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused;

    private void Start()
    {
        gameIsPaused = false;    
    }
}
