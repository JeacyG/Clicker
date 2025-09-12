using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemAmount;
    [SerializeField] private Image craftingBar;

    private bool activated = false;

    private Coroutine craftCoroutine = null;

    private void Awake()
    {
        if (item == null)
            return;
        
        // activated = PlayerPrefs.GetInt(item.itemName + "Activated") == 1;
        gameObject.SetActive(true);
        craftingBar.enabled = false;
        
        itemName.text = item.name;
        Inventory.Instance.OnInventoryChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Inventory.Instance.OnInventoryChanged -= RefreshUI;
    }

    public void OnClick()
    {
        if (craftCoroutine != null)
            return;

        if (!CanCraft())
            return;
        
        Craft();
    }

    private bool CanCraft()
    {
        CraftingRecipeSO recipe = item.recipe;
        
        if (recipe == null)
            return false;
        
        foreach (CraftingRecipeSO.Ingredient ingredient in recipe.ingredients)
        {
            if (!Inventory.Instance.Contains(ingredient.item, ingredient.quantity))
            {
                return false;
            }
        }
        
        return true;
    }

    private void Craft()
    {
        CraftingRecipeSO recipe = item.recipe;
        
        if (recipe == null)
            return;

        craftCoroutine = StartCoroutine(CraftCoroutine());
    }

    private IEnumerator CraftCoroutine()
    {
        CraftingRecipeSO recipe = item.recipe;
        float timeRemaining = recipe.timeToCraft;
        
        craftingBar.enabled = true;

        while (timeRemaining > 0)
        {
            craftingBar.fillAmount = timeRemaining / recipe.timeToCraft;
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        
        craftingBar.enabled = false;
        
        foreach (CraftingRecipeSO.Ingredient ingredient in recipe.ingredients)
        {
            Inventory.Instance.Remove(ingredient.item, ingredient.quantity);
        }
        
        Inventory.Instance.Add(recipe.result, recipe.resultQuantity);
        
        craftCoroutine = null;
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
