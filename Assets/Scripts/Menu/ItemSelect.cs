using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class ItemSelect : MonoBehaviour
    {
        public Menu mainMenu;      
              
        public GameObject weaponPrefab;
        public GameObject trinketPrefab;

        public GameObject weaponSelectHolder;
        public GameObject trinketSelectHolder;

        public ItemDropZone weaponSlot;

        public List<ItemDropZone> trinketSlots;

        //public int perkPoints = 5;
        //public TMP_Text perkPointsUI;

        public List<Item> itemChoices;

        // Use this for initialization
        void Start()        {                  

            var items = DAL.ObjectDAL.GetAllItems();

            foreach (Item item in items.itemList)
            {
                if(GameManager.instance.progressManager.CheckItemUnlock(item.itemName))
                {
                    if (item.type == Item.ItemType.Trinket)
                    {
                        var itemPrefab = Instantiate(trinketPrefab);
                        itemPrefab.transform.SetParent(trinketSelectHolder.transform, false);
                        var itemScript = itemPrefab.GetComponent<ItemPrefab>();

                        itemScript.PopulateItemPrefab(item, this);
                    }
                    else if (item.type == Item.ItemType.Weapon)
                    {
                        var itemPrefab = Instantiate(weaponPrefab);
                        itemPrefab.transform.SetParent(weaponSelectHolder.transform, false);
                        var itemScript = itemPrefab.GetComponent<ItemPrefab>();

                        itemScript.PopulateItemPrefab(item, this);
                    }
                    else
                    {
                        Debug.LogError("Unkown Item Type");
                    }
                }
            }
        }

        public void HighlightSlots(Item.ItemType type, bool on)
        {
            if(type == Item.ItemType.Weapon)
            {
                weaponSlot.HighlightSlot(on);
            }
            else
            {
                foreach(ItemDropZone slot in trinketSlots)
                {
                    slot.HighlightSlot(on);
                }
            }
        }

        public void AddItem(ItemPrefab item)
        {
            itemChoices.Add(item.item);
        }

        public void RemoveItem(ItemPrefab item)
        {
            itemChoices.Remove(item.item);
        }

        public void Select()
        {
            GameManager.instance.itemManager.activeItems = itemChoices;
            GameManager.instance.itemManager.ActivateItems();
            mainMenu.StartGame();
        }
    }
}
