using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemShop : MonoBehaviour
{
    [System.Serializable]
    public class LevelList
    {
        public int Level { get; private set; }
        public List<Equipment> Items { get; private set; }

        public LevelList(int level)
        {
            Level = level;
            Items = new List<Equipment>();
        }
    }

    public int ShopLevel;
    public List<GameObject> ItemSlots;
    public List<Equipment> Items;

    private Dictionary<int, LevelList> levels = new Dictionary<int, LevelList>();

    void Start()
    {
        foreach (Equipment item in Items)
        {
            if (!levels.ContainsKey(item.Level))
            {
                levels[item.Level] = new LevelList(item.Level);
            }
            levels[item.Level].Items.Add(item);
        }

        List<Equipment> filteredItems = levels.ContainsKey(ShopLevel) ? levels[ShopLevel].Items : new List<Equipment>();

        System.Random rng = new System.Random();
        filteredItems = filteredItems.OrderBy(x => rng.Next()).ToList();

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (i < filteredItems.Count)
            {
                ItemSlots[i].GetComponent<ItemSlot>().SetItem(filteredItems[i]);
            }
        }
    }
}