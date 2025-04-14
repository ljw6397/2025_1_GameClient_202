using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/item")]

public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();

    private Dictionary<int, ItemSO> itemsByld;
    private Dictionary<string, ItemSO> itemsByName;

    public void Initialize()
    {
        itemsByld = new Dictionary<int, ItemSO>();
        itemsByName = new Dictionary<string, ItemSO>();

        foreach (var item in items)
        {
            itemsByld[item.id] = item;
            itemsByName[item.itemName] = item;
        }
    }

    public ItemSO GetItemByld(int id)
    {
        if (itemsByld == null)
        {
            Initialize();
        }
        if (itemsByld.TryGetValue(id, out ItemSO item))
            return item;
        return null;
    }

    public ItemSO GetItemByName(int name)
    {
        if (itemsByName == null)
        {
            Initialize();
        }
        if (itemsByld.TryGetValue(name, out ItemSO item))
            return item;
        return null;
    }

    public List<ItemSO> GetItemByType(ItemType type)
    {
        return items.FindAll(item => item.ItemType == type);
    }

}
