using UnityEngine;

[CreateAssetMenu(menuName = "Game/CraftingRecipe")]
public class CraftingRecipeSO : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient
    {
        public ItemSO item;
        public int quantity;
    }
    
    public Ingredient[] ingredients;
    public ItemSO result;
    public int resultQuantity = 1;
    public float timeToCraft = 0f;
}
