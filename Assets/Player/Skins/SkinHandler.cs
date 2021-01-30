using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    public CupSkin[] skins;

    public CupSkin GetSkin(string name)
    {
        foreach(var s in skins)
        {
            if (s.Name == name)
                return s;
        }

        throw new System.Exception("Skin [" + name + "] does not exist");
    }

    public CupSkin GetNextSkin(string current)
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
