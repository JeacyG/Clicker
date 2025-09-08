using System;
using System.Collections.Generic;

public class Inventory
{
    private Dictionary<ItemSO, int> items = new Dictionary<ItemSO, int>();
    public event Action<ItemSO, int> OnInventoryChanged;
    
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Inventory();
            }
            return instance;
        }
    }

    public void Add(ItemSO item, int amount = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items.Add(item, amount);
        }
        OnInventoryChanged?.Invoke(item, items[item]);
    }

    public void Remove(ItemSO item, int amount = 1)
    {
        items[item] = Math.Max(items[item] - amount, 0);
        OnInventoryChanged?.Invoke(item, items[item]);
    }
}
