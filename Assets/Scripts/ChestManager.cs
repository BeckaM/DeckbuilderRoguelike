﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{

    public class ChestManager : MonoBehaviour
    {
        private enum Content { Gold, Consumable, Card };
        private Content content;
        private Card cardReward;
        private int goldReward;
        private string subText;

        internal void PopulateChest(int level)
        {
            var rand = UnityEngine.Random.Range(0, 10);
            if (rand == 0)
            {
                content = Content.Card;
                cardReward = DAL.ObjectDAL.GetRandomCard(level, level + 1);
                subText = "It contains a powerful card. Add it to your deck?";
            }
            else if (rand > 0 && rand < 6)
            {
                content = Content.Consumable;
                cardReward = DAL.ObjectDAL.GetRandomConsumable(level);
                subText = "It contains a consumable. Add it to your deck?";

            }
            else
            {
                content = Content.Gold;
                goldReward = UnityEngine.Random.Range(5 + level, 10 + level);

                subText = "It contains some Gold.";
            }
        }

        internal void OpenChest()
        {
            if (content == Content.Gold)
            {
                GameManager.instance.modalPanel.Chest("You found a chest!", subText, goldReward, AddReward);
            }
            else
            {
                GameManager.instance.modalPanel.Chest("You found a chest!", subText, cardReward, AddReward, DeclineReward);
            }
        }

        private void DeclineReward()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void AddReward()
        {
            if (content == Content.Gold)
            {
                GameManager.instance.ModifyGold(goldReward);
            }
            else
            {
                DeckManager.player.AddCardtoDeck(cardReward.cardName);

            }
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }
    }
}
