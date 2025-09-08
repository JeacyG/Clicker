using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemAmount;

    private bool activated = false;

    private void Awake()
    {
        if (item == null)
            return;
        
        activated = PlayerPrefs.GetInt(item.itemName + "Activated") == 1;
        gameObject.SetActive(activated);
        
        itemName.text = item.name;
        Inventory.Instance.OnInventoryChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        Inventory.Instance.OnInventoryChanged -= RefreshUI;
    }

    private void RefreshUI(ItemSO inItem, int amount)
    {
        if (inItem != item)
        {
            return;
        }

        if (!activated)
            activated = true;
        
        itemAmount.text = amount.ToString();
    }
}
