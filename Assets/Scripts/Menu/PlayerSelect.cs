﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class PlayerSelect : MonoBehaviour
    {

        public List<Button> playerButtons;
        public Menu mainMenu;

        public GameObject selectPanel;
        public GameObject createPlayerPanel;
        public int selectedPlayer;
        public TMP_Text playerName;

        // Use this for initialization
        void Start()
        {
            int playerNr = 1;
            foreach (Button pButton in playerButtons)
            {
                var buttonText = pButton.GetComponentInChildren<TMP_Text>();
                if (DAL.PlayerSaveDAL.PlayerExists(playerNr))
                {                    
                    buttonText.text = DAL.PlayerSaveDAL.GetMoon(playerNr);
                    pButton.onClick.AddListener(mainMenu.HidePlayerSelect);
                }
                else
                {
                    buttonText.text = "Create New";
                }

                playerNr++;
            }

        }

        public void SelectPlayer(int player)
        {
            if (DAL.PlayerSaveDAL.PlayerExists(player))
            {
                DAL.PlayerSaveDAL.LoadPlayer(player);
            }
            else
            {
                selectPanel.SetActive(false);
                createPlayerPanel.SetActive(true);
                selectedPlayer = player;
            }
        }

        public void CreatePlayer()
        {
            DAL.PlayerSaveDAL.CreateNewPlayer(selectedPlayer, playerName.text);
        }

    }
}
