using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public CraftingRecipeSO recipe;
    public Sprite icon;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = this.name;
    }
}
