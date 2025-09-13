using System;
using System.Collections.Generic;
using UnityEngine;

public enum SortingType
{
    Name,
    Amount,
    Tier
}

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject itemCellPrefab;
    [SerializeField] private Transform inventory;
    
    private SortingType sortingType = SortingType.Tier;
    private bool bAscending = true;
    
    private void Awake()
    {
        Inventory.Instance.OnItemAdded += AddItemCell;
        Inventory.Instance.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnDestroy()
    {
        Inventory.Instance.OnItemAdded -= AddItemCell;
        Inventory.Instance.OnInventoryChanged -= OnInventoryChanged;
    }

    private void AddItemCell(ItemSO item)
    {
        ItemController controller = Instantiate(itemCellPrefab, inventory).GetComponent<ItemController>();
        controller.Initialize(item);
        Inventory.Instance.Push(controller);
        SortItems();
    }

    public void OnSortingValueChanged(int i)
    {
        switch (i)
        {
            case 0:
                ChangeSorting(SortingType.Name, bAscending);
                break;
            case 1:
                ChangeSorting(SortingType.Amount, bAscending);
                break;
            case 2:
                ChangeSorting(SortingType.Tier, bAscending);
                break;
        }
    }

    public void OnAscendingValueChanged(bool b)
    {
        ChangeSorting(sortingType, b);
    }

    private void ChangeSorting(SortingType inSortingType, bool inAscending)
    {
        if (inSortingType == sortingType && inAscending == bAscending)
            return;
        
        bAscending = inAscending;
        sortingType = inSortingType;
        
        SortItems();
    }

    private void OnInventoryChanged(ItemSO item, int amount)
    {
        SortItems();
        
        if (Inventory.Instance.Contains(item, 1))
        {
            foreach (ItemSO itemToUnlock in item.itemsToUnlock)
            {
                Inventory.Instance.Add(itemToUnlock, 0);
            }
        }
    }

    private void SortItems()
    {
        ItemSO[] items = Inventory.Instance.GetItems();

        switch (sortingType)
        {
            case SortingType.Name:
                if (bAscending)
                    System.Array.Sort(items, (a, b) =>
                        string.Compare(a.itemName, b.itemName, StringComparison.Ordinal));
                else
                    System.Array.Sort(items, (a, b) =>
                        string.Compare(b.itemName, a.itemName, StringComparison.Ordinal));
                break;
            case SortingType.Amount:
                if (bAscending)
                    System.Array.Sort(items, (a, b) =>
                        Inventory.Instance.GetAmount(a).CompareTo(Inventory.Instance.GetAmount(b)));
                else
                    System.Array.Sort(items, (a, b) =>
                        Inventory.Instance.GetAmount(b).CompareTo(Inventory.Instance.GetAmount(a)));
                break;
            case SortingType.Tier:
                if (bAscending)
                    System.Array.Sort(items, (a, b) =>
                        a.tier.CompareTo(b.tier));
                else
                    System.Array.Sort(items, (a, b) =>
                        b.tier.CompareTo(a.tier));
                break;
        }
        
        for (int i = 0; i < items.Length; i++)
        {
            ItemController controller = Inventory.Instance.GetController(items[i]);
            controller.transform.SetSiblingIndex(i);
        }
    }
}
