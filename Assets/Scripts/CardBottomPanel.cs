using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardBottomPanel : MonoBehaviour
    {
        public GameObject damageIcon;
        public TMP_Text damageText;
        public int damageNumber;

        public GameObject healIcon;
        public TMP_Text healText;
        public int healNumber;

        public GameObject wardIcon;
        public TMP_Text wardText;
        public int wardNumber;

        public GameObject durationPanel;
        public GameObject durationInstantIcon;
        public GameObject durationPermanentIcon;

        public GameObject durationIcon;
        public TMP_Text durationText;
                
        public TMP_Text costText;

        public int cardEffectCount;

        public void ShowDamageIcon(int damage)
        {
            
            damageIcon.SetActive(true);            
            damageNumber = damageNumber + damage;
            damageText.text = damageNumber.ToString();
            cardEffectCount++;
        }

        public void ShowHealIcon(int heal)
        {
            healIcon.SetActive(true);
            healNumber = healNumber + heal;
            healText.text  =healNumber.ToString();
            cardEffectCount++;
        }

        public void ShowWardIcon(int ward)
        {
            wardIcon.SetActive(true);
            wardNumber = wardNumber + ward;
            wardText.text = wardNumber.ToString();
            cardEffectCount++;
        }

        public void ShowInstantDurationIcon()
        {
            durationPanel.SetActive(true);
            durationInstantIcon.SetActive(true);            
        }

        public void ShowPermanentDurationIcon()
        {
            durationPanel.SetActive(true);
            durationPermanentIcon.SetActive(true);
        }

        public void ShowDurationIcon(int duration)
        {
            durationIcon.SetActive(true);                        
            durationText.text = duration.ToString();
        }

        public void SetCostIcon(int cost)
        {
            costText.text = cost.ToString();
        }

        public void SetEffectSize()
        {

        }



    }
}