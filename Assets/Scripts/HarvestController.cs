using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HarvestController : MonoBehaviour
{
    [SerializeField] private HarvesterSO harvestableItems;
    [SerializeField] private float timeToHarvest = 1f;
    [SerializeField] private Image harvestBar;
    [SerializeField] private Button harvestButton;

    [SerializeField] private TMP_Text debugNameText;
    
    private Coroutine harvestCoroutine;
    private ItemSO currentItem;

    private void Awake()
    {
        ChangeToItem(GetRandomHarvestItem());
    }

    public void OnClick()
    {
        if (harvestCoroutine == null)
        {
            harvestCoroutine = StartCoroutine(HarvestCoroutine());
        }
    }

    private IEnumerator HarvestCoroutine()
    {
        harvestButton.interactable = false;
        float timeRemaining = timeToHarvest;

        while (timeRemaining > 0)
        {
            harvestBar.fillAmount = timeRemaining / timeToHarvest;
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        
        Inventory.Instance.Add(currentItem);
        ChangeToItem(GetRandomHarvestItem());
        harvestButton.interactable = true;
        harvestBar.fillAmount = 1f;
        harvestCoroutine = null;
    }

    private void ChangeToItem(ItemSO item)
    {
        if (!item)
            return;
        
        currentItem = item;
        debugNameText.text = item.itemName;
    }

    private ItemSO GetRandomHarvestItem()
    {
        if (!harvestableItems)
            return null;
        
        int totalWeight = 0;

        foreach (HarvesterSO.HarvestableItem entry in harvestableItems.possibleItems)
        {
            totalWeight += entry.weight;
        }
        
        int randomWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;
        
        foreach (HarvesterSO.HarvestableItem entry in harvestableItems.possibleItems)
        {
            currentWeight += entry.weight;
            if (currentWeight >= randomWeight)
            {
                return entry.item;
            }
        }
        
        return null;
    }
}
