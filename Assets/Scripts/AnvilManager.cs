﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{

    public class AnvilManager : MonoBehaviour
    {

        internal void OpenAnvil()
        {
            GameManager.instance.modalPanel.Anvil("You found an Anvil!", "",  Decline);
            GameManager.instance.progressManager.FoundShrine();
        }

        private void Decline()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Destruction()
        {            
            GameManager.instance.deckPanel.DestroyCardPanel();

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Upgrade()
        {
            Debug.Log("Prayer of Evolution triggered");
            GameManager.instance.deckPanel.UpgradeCardPanel();

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }
    }
}

