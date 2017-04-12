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

        public PerkButton selectedPerk;
        public TMP_Text selectedPerkNameText;
        public TMP_Text selectedPerkEffectText;
        public Button selectButton;

        public GameObject perkButtonObj;
        public GameObject perkPanel;
        public GameObject perkChoicePanel;

        public int perkPoints = 5;
        public TMP_Text perkPointsUI;

        public List<Perk> perkChoices;

        // Use this for initialization
        void Start()
        {
            selectButton.interactable = false;
            perkPointsUI.text = perkPoints.ToString();

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


        public void SelectPerk(PerkButton perkButton)
        {
            selectedPerk = perkButton;
            selectedPerkEffectText.text = perkButton.perk.perkEffectText;
            selectedPerkNameText.text = perkButton.perk.perkName;

            selectButton.interactable = true;
        }

        public void AddRemovePerk()
        {
            if (selectedPerk.active)
            {
                selectedPerk.transform.SetParent(perkPanel.transform);
                selectedPerk.active = false;
                perkPoints += selectedPerk.perk.perkCost;
                perkPointsUI.text = perkPoints.ToString();
            }
            else
            {
                if (perkPoints >= selectedPerk.perk.perkCost)
                {
                    perkChoices.Add(selectedPerk.perk);
                    selectedPerk.transform.SetParent(perkChoicePanel.transform);
                    selectedPerk.active = true;
                    perkPoints -= selectedPerk.perk.perkCost;
                    perkPointsUI.text = perkPoints.ToString();
                }
            }
        }


        public void Select()
        {
            mainMenu.StartGame();
        }


    }
}
