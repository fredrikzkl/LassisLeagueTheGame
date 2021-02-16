using UnityEngine;
using System.Collections;
using UnityEditor;

public class BallSelector : ItemSelector
{
    public override void RegisterNewItem(Item newCurrentItem)
    {
        vsModeSettings.UpdateBall(newCurrentItem.Name);
    }
}
