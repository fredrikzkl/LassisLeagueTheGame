using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapSelector : ItemSelector
{
    public override void RegisterNewItem(Item newCurrentItem)
    {
        vsModeSettings.UpdateMap(newCurrentItem.Name);
    }
}
