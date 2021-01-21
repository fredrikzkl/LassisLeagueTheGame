using UnityEngine;
using System.Collections;

public static class SessionData 
{
    public static int Player1Wins;
    public static int Player2Wins;

    public static void ClearSession()
    {
        Player1Wins = 0;
        Player2Wins = 0;
    }
}
