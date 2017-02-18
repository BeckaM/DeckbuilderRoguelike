using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace Assets.Scripts
{

    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
            if (eventData.pointerDrag == null)
                return;

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
            {
                d.placeholderParent = this.transform;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit");
            if (eventData.pointerDrag == null)
                return;

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null && d.placeholderParent == this.transform)
            {
                d.placeholderParent = d.parentToReturnTo;
            }
        }



        public void OnDrop(PointerEventData eventData)
        {


            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
            {


                CardManager card = eventData.pointerDrag.GetComponent<CardManager>();

                if (card.isPlayable && card.isDragable)
                {
                    d.parentToReturnTo = this.transform;
                    CardgameManager.instance.PlaceCard(card);

                    //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
                }

            }


        }

    }
}