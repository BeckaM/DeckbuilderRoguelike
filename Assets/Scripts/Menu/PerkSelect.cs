using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class PerkSelect : MonoBehaviour
    {
        public Menu mainMenu;
                                
        public Perk selectedPerk;        
        public TMP_Text selectedPerkText;
        public Button selectButton;

        public GameObject perkButtonObj;
        public GameObject perkPanel;

        // Use this for initialization
        void Start()
        {
            selectButton.interactable = false;

            var perks = DAL.ObjectDAL.GetAllPerks();

            foreach (Perk perk in perks.perkList)
            {
                var perkButton = Instantiate(perkButtonObj);
                perkButton.transform.SetParent(perkPanel.transform, false);
                var perkScript = perkButton.GetComponent<PerkButton>();
                perkScript.perkSelect = this;

                perkScript.PopulatePerkButton(perk);

                perkScript.LockPerk(!GameManager.instance.progressManager.CheckPerkUnlock(perk.perkName));
            }
        }


        public void SelectPerk(Perk perk)
        {
            selectedPerk = perk;            
            selectedPerkText.text = perk.perkEffectText;

            selectButton.interactable = true;
        }


        public void Select()
        {
           
            mainMenu.StartGame();
        }


    }
}
