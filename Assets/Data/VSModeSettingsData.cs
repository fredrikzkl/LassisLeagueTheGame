using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class VSModeSettingsData 
{
    [EnumMember(Value = "player1Type")]
    public PlayerType player1Type;
    [EnumMember(Value = "player2Type")]
    public PlayerType player2Type;

    [EnumMember(Value = "player1AIDifficulty")]
    public DifficultyLevel player1AIDifficulty;
    [EnumMember(Value = "player2AIDifficulty")]
    public DifficultyLevel player2AIDifficulty;

    public VSModeSettingsData Standard()
    {
        return new VSModeSettingsData
        {
            player1Type = PlayerType.Player,
            player2Type = PlayerType.Player,

            player1AIDifficulty = DifficultyLevel.Easy,
            player2AIDifficulty = DifficultyLevel.Easy
        };
      
    }
}
