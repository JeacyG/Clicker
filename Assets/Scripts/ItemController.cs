using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemSO item;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemAmount;
    [SerializeField] private Image craftingBar;
    [SerializeField] private float timeUntilTooltip = 1.0f;
    [SerializeField] private TooltipController tooltip;
    [SerializeField] private Image highlight;
    
    public ItemSO Item => item;

    private Coroutine craftCoroutine = null;
    private Coroutine tooltipCoroutine = null;

    public void Initialize(ItemSO inItem)
    {
        item = inItem;
        tooltip.Initialize();
        
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
        if (craftCoroutine != null || Inventory.Instance.ManualCrafting)
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

        Inventory.Instance.ManualCrafting = true;
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
        
        Inventory.Instance.ManualCrafting = false;
        craftCoroutine = null;
    }

    private void RefreshUI(ItemSO inItem, int amount)
    {
        if (inItem != item)
        {
            return;
        }
        
        itemAmount.text = amount.ToString();
    }

    public void SetHighlight(bool bActive)
    {
        highlight.gameObject.SetActive(bActive);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipCoroutine = StartCoroutine(TooltipCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
        }
        tooltip.gameObject.SetActive(false);
        if (item.recipe)
        {
            foreach (CraftingRecipeSO.Ingredient ingredient in item.recipe.ingredients)
            {
                ItemController controller = Inventory.Instance.GetController(ingredient.item);
                if (controller)
                    controller.SetHighlight(false);
            }
        }
    }

    private IEnumerator TooltipCoroutine()
    {
        yield return new WaitForSeconds(timeUntilTooltip);
        tooltip.gameObject.SetActive(true);
        if (item.recipe)
        {
            foreach (CraftingRecipeSO.Ingredient ingredient in item.recipe.ingredients)
            {
                ItemController controller = Inventory.Instance.GetController(ingredient.item);
                if (controller)
                    controller.SetHighlight(true);
            }
        }
        
        tooltipCoroutine = null;
    }
}
