using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    class DungeonFloor: MonoBehaviour,IPointerDownHandler
    {
        public Player player;
        public void OnPointerDown(PointerEventData eventData)
        {
            player.MovePlayerTowards(eventData.position);
        }

       

    }
}
