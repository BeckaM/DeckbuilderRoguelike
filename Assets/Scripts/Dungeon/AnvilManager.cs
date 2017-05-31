using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using cakeslice;

namespace Assets.Scripts
{

    public class AnvilManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Outline outline;

        internal void OnEnable()
        {
            outline.enabled = false;
        }

        internal void OpenAnvil()
        {
            GameManager.instance.dungeonUI.modalPanel.Anvil("You found an Anvil!", "",  Decline);
            GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Shrines_Opened,1);
        }

        private void Decline()
        {            
            GameManager.instance.doingSetup = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            outline.enabled = true;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            outline.enabled = false;
        }
             
    }
}


