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

        public ItemButton selectedItem;
        public TMP_Text selectedItemNameText;
        public TMP_Text selectedItemEffectText;
        public Button selectButton;

        public GameObject itemButtonObj;
        public GameObject itemPanel;
        public GameObject itemChoicePanel;

        //public int perkPoints = 5;
        //public TMP_Text perkPointsUI;

        public List<Item> itemChoices;

        // Use this for initialization
        void Start()
        {
            selectButton.interactable = false;
         //   perkPointsUI.text = perkPoints.ToString();

            var items = DAL.ObjectDAL.GetAllItems();

            foreach (Item item in items.itemList)
            {
                var itemButton = Instantiate(itemButtonObj);
                itemButton.transform.SetParent(itemPanel.transform, false);
                var itemScript = itemButton.GetComponent<ItemButton>();
                
                itemScript.PopulateItemButton(item);
                               
            }
        }


        public void SelectItem(ItemButton itemButton)
        {
            selectedItem = itemButton;
            selectedItemEffectText.text = itemButton.item.itemEffectText;
            selectedItemNameText.text = itemButton.item.itemName;

            selectButton.interactable = true;
        }

        public void AddRemoveItem()
        {
            if (selectedItem.active)
            {
                selectedItem.transform.SetParent(itemPanel.transform);
                selectedItem.active = false;
                //perkPoints += selectedItem.item.perkCost;
                //perkPointsUI.text = perkPoints.ToString();
                itemChoices.Remove(selectedItem.item);
            }
            else
            {
                
                    itemChoices.Add(selectedItem.item);
                    selectedItem.transform.SetParent(itemChoicePanel.transform);
                    selectedItem.active = true;
                    //perkPoints -= selectedItem.item.perkCost;
                    //perkPointsUI.text = perkPoints.ToString();
                
            }
        }


        public void Select()
        {
            GameManager.instance.itemManager.activeItems = itemChoices;
            GameManager.instance.itemManager.ActivateItems();
            mainMenu.StartGame();
        }


    }
}
