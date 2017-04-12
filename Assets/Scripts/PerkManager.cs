
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
                default:
                    {
                        Debug.LogError("Perk: " + perk.perkName + " . Not implemented!");
                        break;
                    }

            }

        }

    }
}
