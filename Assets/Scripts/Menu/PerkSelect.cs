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

        public List<ClassButton> perkButtons;
        
        public Perk selectedPerk;        
        public TMP_Text selectedPerkText;
        public Button selectButton;
        
        // Use this for initialization
        void Start()
        {
            selectButton.interactable = false;
                        
        }


        public void SelectClass(Perk perk)
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
