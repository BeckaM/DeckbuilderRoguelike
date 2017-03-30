using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Assets.Scripts.Menu
{
    public class PerkButton : MonoBehaviour
    {
        public PerkSelect perkSelect;
        public GameObject lockPanel;
        public TMP_Text perkName;

        public Perk perk;

        public void selectPerk()
        {
            perkSelect.SelectPerk(perk);
        }

        public void PopulatePerkButton(Perk perk)
        {
            this.perk = perk;
            perkName.text = perk.perkName;
        }

        public void LockPerk(bool locked)
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

