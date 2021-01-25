using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Cup Skin Preset", menuName = "Scriptables/Cup Skin Preset Preset", order = 1)]
public class CupSkin : ScriptableObject
{
    public string Name;

    public Color playerColor;
    public Material standardMaterial;
    public Material hitMaterial;
    public Material rimMaterial;
}
