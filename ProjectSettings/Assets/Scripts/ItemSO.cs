using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/item")]

public class ItemSO : ScriptableObject
{
    public int id;
    public string itemName;
    public string nameEng;
    public string description;
    public ItemType itmeType;
    public int price;
    public int power;
    public bool isStackable;
    public Sprite icon;
    internal ItemType ItemType;
    internal int level;

    public override string ToString()
    {
        return $"[{id}] {itemName} ({itmeType}) - 가격 : {price}골드, 속성 : {power}";
    }

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(nameEng) ? itemName : nameEng; }
    }
}
