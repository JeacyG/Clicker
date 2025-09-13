using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Harvester", menuName = "Game/Harvester")]
public class HarvesterSO : ScriptableObject
{
    [Serializable]
    public struct HarvestableItem
    {
        public ItemSO requirement;
        public ItemSO item;
        public int weight;
    }

    public string harvesterName;
    public HarvestableItem[] possibleItems;

    private void OnValidate()
    {
        harvesterName = this.name;
    }
}
