﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/*
 * Brukes som en generel abstrakt klasse for å håndtere meny-valg 
 */

public class Item
{
    public string Name;
    public Material Material;
}

public class ItemSelector : MonoBehaviour
{
    public Item[] Items;
    private Item CurrentItem;

    private MeshRenderer MeshRender;

    private void Start()
    {
        if(gameObject.GetComponent<MeshRenderer>() != null)
        {
            MeshRender = gameObject.GetComponent<MeshRenderer>();
        }
    }

    public Item NextItem()
    {
        if(Items.Length < 1)
        {
            Debug.LogWarning($"{gameObject}'s itemlist is empty");
            return null;
        }

        for (int i = 0; i < Items.Length; i++)
        {
            if (CurrentItem == Items[i])
            {
                int temp = i + 1;

                if (temp == Items.Length)
                    temp = 0;

                var nextItem = Items[temp];

                ApplyItem(nextItem);
                return nextItem;
            }
        }
        return null;
    }

    public Item GetItem(string name)
    {
        foreach(var i in Items)
        {
            if (i.Name == name)
                return i;
        }
        return null;
    }

    public void ApplyItem(Item newCurrentItem)
    {
        if (MeshRender != null)
            MeshRender.material = newCurrentItem.Material;

        CurrentItem = newCurrentItem;
        //vsModeSettings.UpdateMap(current.Name);
    }

    public void SetItem(string name)
    {
        foreach(var i in Items)
        {
            if(i.Name.ToLower() == name.ToLower())
            {
                ApplyItem(i);
                return;
            }
        }
        Debug.LogWarning($"Itemlist for {gameObject} does not contain: '{name}'");
        if(Items.Length == 0)
        {
            Debug.LogWarning($"{gameObject}'s itemlist is empty");
            return;
        }
        ApplyItem(Items[0]);
    }

    public Item GetRandomItem()
    {
        List<Item> itemsWithoutRandom = new List<Item>();
        foreach (var item in Items)
        {
            if (item.Name != "Random")
                itemsWithoutRandom.Add(item);
        }
        System.Random rng = new System.Random();
        var randomItemIndex = rng.Next(0, itemsWithoutRandom.Count);
        return itemsWithoutRandom[randomItemIndex];
    }


    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1") && !IsPointerOverUIObject())
            NextItem();
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}