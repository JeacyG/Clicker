using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private ItemController itemController;
    [SerializeField] private Transform craftingRecipeTooltip;
    [SerializeField] private GameObject craftingRecipeEntryPrefab;

    public void Initialize()
    {
        CraftingRecipeSO craftingRecipe = itemController.Item.recipe;

        if (!craftingRecipe)
            return;

        foreach (CraftingRecipeSO.Ingredient ingredient in craftingRecipe.ingredients)
        {
            GameObject entry = Instantiate(craftingRecipeEntryPrefab, craftingRecipeTooltip);
            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();
            
            texts[0].text = ingredient.item.itemName;
            texts[1].text = "x " + ingredient.quantity;
        }
    }
}
