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

        public GameObject manaIncreaseIcon;
        public TMP_Text manaIncText;
        public int manaIncNumber;

        public GameObject damageIncreaseIcon;
        public TMP_Text damageIncText;
        public int damageIncNumber;

        public GameObject durationPanel;
        public GameObject durationInstantIcon;
        public GameObject durationPermanentIcon;

        public GameObject durationIcon;
        public TMP_Text durationText;
                
        public TMP_Text costText;
                
        public int cardEffectCount;
        

        public void PopulateBottomPanel(Card card)
        {
            //set cost icon
           SetCostIcon(card.cost);

            //set card duration icon
            if (card.cardDuration == 0)
            {
                ShowInstantDurationIcon();
            }
            else if (card.cardDuration > 0)
            {
                ShowDurationIcon(card.cardDuration);
            }
            else
            {
                ShowPermanentDurationIcon();
            }

            //set card effect icons
            foreach (CardEffect effect in card.effects)
            {
                if (effect.effect == CardEffect.Effect.DealDamage)
                {
                    ShowDamageIcon(effect.value);
                }
                else if (effect.effect == CardEffect.Effect.Heal)
                {
                    ShowHealIcon(effect.value);
                }
                else if (effect.effect == CardEffect.Effect.ReduceDamage)
                {
                    ShowWardIcon(effect.value);
                }
                else if (effect.effect == CardEffect.Effect.AddMaxMana)
                {
                    ShowManaIncreaseIcon(effect.value);
                }
                else if (effect.effect == CardEffect.Effect.IncreaseDamage)
                {
                    ShowDamageIncreaseIcon(effect.value);
                }                
            }

            SetEffectSize();
        }

        public void ShowBottomPanel(bool show)
        {
            this.gameObject.SetActive(show);
        }
        
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

        public void ShowManaIncreaseIcon(int manaInc)
        {
            manaIncreaseIcon.SetActive(true);
            manaIncNumber = manaIncNumber + manaInc;
            manaIncText.text = "+" + manaIncNumber.ToString();
            cardEffectCount++;
        }

        public void ShowDamageIncreaseIcon(int damageInc)
        {
            damageIncreaseIcon.SetActive(true);
            damageIncNumber = damageIncNumber + damageInc;
            damageIncText.text = "+" + damageIncNumber.ToString();
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