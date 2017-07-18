using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class Item
    {
        public string itemName;
        public string itemEffectText;
        public enum ItemType {Weapon, Trinket};
        public ItemType type;
        public int itemSprite;

        public string conditionText;
        public ProgressManager.Metric condition;
        public int conditionValue;
    }


    [Serializable]
    public class ItemWrapper
    {
        public List<Item> itemList;
    }
}