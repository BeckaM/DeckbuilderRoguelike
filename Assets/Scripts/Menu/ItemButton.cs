using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Assets.Scripts.Menu
{
    public class ItemButton : MonoBehaviour
    {
        public ItemSelect itemSelect;
        public GameObject lockPanel;
        public TMP_Text itemName;
       // public TMP_Text perkCost;

        public Item item;
        
        public bool active;

        public void selectItem()
        {
            itemSelect.SelectItem(this);
        }

        public void PopulateItemButton(Item item)
        {
            this.item = item;
            itemName.text = item.itemName;
           // perkCost.text = "Cost: " + perk.perkCost.ToString();
        }

        public void LockItem(bool locked)
        {
            if (locked)
            {
                lockPanel.SetActive(true);
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Button>().interactable = true;
                lockPanel.SetActive(false);
            }
        }
    }
}

