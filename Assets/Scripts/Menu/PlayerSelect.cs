using System.Collections;
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
            selectPanel.SetActive(true);
            createPlayerPanel.SetActive(false);
            int playerNr = 1;
            foreach (Button pButton in playerButtons)
            {
                var buttonText = pButton.GetComponentInChildren<TMP_Text>();
                if (DAL.PlayerSaveDAL.PlayerExists(playerNr))
                {                    
                    buttonText.text = DAL.PlayerSaveDAL.GetPlayerName(playerNr);                    
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
                mainMenu.ShowStartScreen();
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
            DAL.PlayerSaveDAL.LoadPlayer(selectedPlayer);
            mainMenu.ShowStartScreen();
        }

        public void CloseNewPlayerPanel()
        {
            createPlayerPanel.SetActive(false);
            selectPanel.SetActive(true);
        }

    }
}
