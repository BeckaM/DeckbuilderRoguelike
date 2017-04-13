
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts
{

    public class PerkManager
    {
        public List<Perk> activePerks;

        public int bonusInitialDraw;
        public float goldIncrease=1.0F;

        public void ActivatePerks()
        {
            foreach (Perk perk in activePerks)
            {
                ActivatePerk(perk);

            }
        }

        private void ActivatePerk(Perk perk)
        {
            switch (perk.perkName)
            {
                case "Taste for Blood":
                    {
                        GameManager.instance.maxLife += 5;
                        GameManager.instance.lifeHolder += 5;
                        break;
                    }
                case "Thirst for Blood":
                    {
                        GameManager.instance.maxLife += 10;
                        GameManager.instance.lifeHolder += 10;
                        break;
                    }
                case "Wisdom From Below":
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
                        Debug.LogError("Perk: " + perk.perkName + " . Not implemented!");
                        break;
                    }
            }
        }

    }
}
