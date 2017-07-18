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

        public GameObject weaponSlot;

        public GameObject trinketSlot1;
        public GameObject trinketSlot2;
        public GameObject trinketSlot3;
        public GameObject trinketSlot4;


        //public int perkPoints = 5;
        //public TMP_Text perkPointsUI;

        public List<Item> itemChoices;

        // Use this for initialization
        void Start()        {                  

            var items = DAL.ObjectDAL.GetAllItems();

            foreach (Item item in items.itemList)
            {
                if (item.type == Item.ItemType.Trinket)
                {
                    var itemPrefab = Instantiate(trinketPrefab);
                    itemPrefab.transform.SetParent(trinketSelectHolder.transform, false);
                    var itemScript = itemPrefab.GetComponent<ItemPrefab>();

                    itemScript.PopulateItemButton(item);
                }
                else if(item.type == Item.ItemType.Weapon)
                {
                    var itemPrefab = Instantiate(weaponPrefab);
                    itemPrefab.transform.SetParent(weaponSelectHolder.transform, false);
                    var itemScript = itemPrefab.GetComponent<ItemPrefab>();

                    itemScript.PopulateItemButton(item);
                }
                else
                {
                    Debug.LogError("Unkown Item Type");
                }
            }
        }
        
        //public void SelectItem(ItemPrefab itemButton)
        //{
        //    selectedItem = itemButton;
        //    selectedItemEffectText.text = itemButton.item.itemEffectText;
        //    selectedItemNameText.text = itemButton.item.itemName;
           
        //}

        //public void AddRemoveItem()
        //{
        //    if (selectedItem.active)
        //    {
        //        selectedItem.transform.SetParent(itemPanel.transform);
        //        selectedItem.active = false;
        //        //perkPoints += selectedItem.item.perkCost;
        //        //perkPointsUI.text = perkPoints.ToString();
        //        itemChoices.Remove(selectedItem.item);
        //    }
        //    else
        //    {
                
        //            itemChoices.Add(selectedItem.item);
        //            selectedItem.transform.SetParent(itemChoicePanel.transform);
        //            selectedItem.active = true;
        //            //perkPoints -= selectedItem.item.perkCost;
        //            //perkPointsUI.text = perkPoints.ToString();
                
        //    }
        //}
        
        public void Select()
        {
            GameManager.instance.itemManager.activeItems = itemChoices;
            GameManager.instance.itemManager.ActivateItems();
            mainMenu.StartGame();
        }
    }
}
