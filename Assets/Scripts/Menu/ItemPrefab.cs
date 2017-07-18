using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Assets.Scripts.Menu
{
    public class ItemPrefab : MonoBehaviour
    {        
        public Image itemImage;
        public TMP_Text itemName;
       // public TMP_Text perkCost;

        public Item item;
        
        public bool active;
             

        public void PopulateItemButton(Item item)
        {
            this.item = item;
            itemName.text = item.itemName;
           // perkCost.text = "Cost: " + perk.perkCost.ToString();
        }
               
    }
}

