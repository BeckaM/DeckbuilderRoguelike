using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts
{
    public class DungeonFloor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Player player;

        public void OnPointerDown(PointerEventData eventData)
        {
            player.moveToggle = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            player.moveToggle = false;
        }
    }
}
