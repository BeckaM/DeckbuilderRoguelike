using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Menu
{

    public class ItemDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            if (eventData.pointerDrag == null)
                return;

            ItemDraggable d = eventData.pointerDrag.GetComponent<ItemDraggable>();
            if (d != null)
            {
                d.placeholderParent = this.transform;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            if (eventData.pointerDrag == null)
                return;

            ItemDraggable d = eventData.pointerDrag.GetComponent<ItemDraggable>();
            if (d != null && d.placeholderParent == this.transform)
            {
                d.placeholderParent = d.parentToReturnTo;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

            ItemDraggable d = eventData.pointerDrag.GetComponent<ItemDraggable>();
            if (d != null)
            {
                d.parentToReturnTo = this.transform;
            }

        }
    }
}
