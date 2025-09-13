using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    private Dictionary<ItemSO, int> items = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, ItemController> itemToController = new Dictionary<ItemSO, ItemController>();
    
    public event Action<ItemSO, int> OnInventoryChanged;
    public event Action<ItemSO> OnItemAdded;

    public bool ManualCrafting = false;
    
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
            OnItemAdded?.Invoke(item);
        }
        OnInventoryChanged?.Invoke(item, items[item]);
    }

    public void Push(ItemController controller)
    {
        itemToController.Add(controller.Item, controller);
    }

    public void Remove(ItemSO item, int amount = 1)
    {
        items[item] = Math.Max(items[item] - amount, 0);
        OnInventoryChanged?.Invoke(item, items[item]);
    }

    public int GetAmount(ItemSO item)
    {
        return items[item];
    }

    public ItemSO[] GetItems()
    {
        return items.Keys.ToArray();
    }

    public ItemController GetController(ItemSO item)
    {
        return itemToController[item];
    }

    public bool Contains(ItemSO item, int amount)
    {
        if (!items.ContainsKey(item))
        {
            return false;
        }
        
        return items[item] >= amount;
    }
}
