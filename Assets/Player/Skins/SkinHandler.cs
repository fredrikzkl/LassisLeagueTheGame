using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public static class SkinHandler 
{
    public static CupSkin[] skins = Resources.FindObjectsOfTypeAll<CupSkin>();

    public static CupSkin GetSkin(string name)
    {
        foreach(var s in skins)
        {
            if (s.Name == name)
                return s;
        }

        Debug.LogWarning("Did not load skins properly, trying again");
        skins = Resources.FindObjectsOfTypeAll<CupSkin>();
        if (skins.Length > 1)
            GetSkin(name);

        throw new Exception("Skin [" + name + "] does not exist");
    }



    public static CupSkin GetNextSkin(string current)
    {
        for (int i = 0; i < skins.Length; i++)
        {
            var temp = skins[i];
            if (current == temp.Name)
            {
                if (i + 1 == skins.Length)
                    return skins[0];
                return skins[i + 1];
            }
        }
        Debug.LogWarning("Skin was not found (GetNextSkin) - Returning [0]");
        return skins[0];
    }
}
