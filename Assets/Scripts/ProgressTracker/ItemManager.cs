
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts
{

    public class ItemManager
    {
        public List<Item> activeItems;

        public int bonusInitialDraw;
        public float goldIncrease=1.0F;

        public void ActivateItems()
        {
            foreach (Item item in activeItems)
            {
                ActivateItem(item);

            }
        }

        private void ActivateItem(Item item)
        {
            switch (item.itemName)
            {
                case "Ring of Fortitude":
                    {
                        GameManager.instance.player.maxLife += 5;
                        GameManager.instance.player.life += 5;
                        break;
                    }
                case "Ring of Greater Fortitude":
                    {
                        GameManager.instance.player.maxLife += 10;
                        GameManager.instance.player.life += 10;
                        break;
                    }
                case "Sword of the Ages":
                    {
                        bonusInitialDraw++;
                        break;
                    }
                case "Mind Over Matter":
                    {
                        bonusInitialDraw+=2;
                        break;
                    }
                case "Fools Greed":
                    {
                        goldIncrease += 0.1F;
                        break;
                    }
                case "Buried Fortune":
                    {
                        goldIncrease += 0.2F;
                        break;
                    }
                default:
                    {
                        Debug.LogError("Item: " + item.itemName + " . Not implemented!");
                        break;
                    }
            }
        }

    }
}
