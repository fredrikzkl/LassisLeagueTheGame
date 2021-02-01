using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Map
{
    public string Name;
    public Material MapImage;
}

public class MapController : MonoBehaviour
{
    public VsModeController vsModeSettings;

    public Map[] maps;

    private Map current;
    private MeshRenderer meshRend;


    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    public Map NextMap()
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if(current == maps[i])
            {
                int temp = i + 1;

                if (temp == maps.Length)
                    temp = 0;

                var nextMap = maps[temp];

                ApplyMap(nextMap);
                return nextMap;
            }
        }
        return null;
    }

    public void SetMap(string name)
    {
        foreach(var m in maps)
        {
            if (m.Name == name)
            {
                ApplyMap(m);
                return;
            }
        }
        //Om banen ikke finnes, eller null, så setter vi random
        ApplyMap(maps[0]);
    }

    public void ApplyMap(Map m)
    {
        meshRend.material = m.MapImage;
        current = m;
        vsModeSettings.UpdateMap(current.Name);
    }


    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1") && !IsPointerOverUIObject()){
            var newMap = NextMap();
            Debug.Log("Clicking on picture, new map: " + newMap.Name);
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public Map GetRandomMap()
    {
        List<Map> mapsWithoutRandom = new List<Map>();
        foreach(var m in maps)
        {
            if (m.Name != "Random")
                mapsWithoutRandom.Add(m);
        }
        System.Random rng = new System.Random();
        var randomMapIndex = rng.Next(0, mapsWithoutRandom.Count);
        return mapsWithoutRandom[randomMapIndex];
    }
}
