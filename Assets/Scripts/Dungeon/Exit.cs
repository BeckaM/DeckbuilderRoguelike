using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Exit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public Outline outline;
        // Use this for initialization
        void Start()
        {
            outline.enabled = false;
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
