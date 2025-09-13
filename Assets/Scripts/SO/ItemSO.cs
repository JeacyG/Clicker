using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int tier;
    public CraftingRecipeSO recipe;
    public ItemSO[] itemsToUnlock;
    public Sprite icon;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = this.name;
    }
}
