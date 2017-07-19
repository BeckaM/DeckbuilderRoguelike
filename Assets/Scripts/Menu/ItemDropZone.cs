using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class ItemDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public string slot;
        public Item.ItemType type;
        public Outline outline;
        public ItemPrefab contains;

        public void HighlightSlot(bool highlighted)
        {
            if (highlighted)
            {
                outline.enabled = true;
            }
            else{
                outline.enabled = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            if (eventData.pointerDrag == null)
                return;

            ItemPrefab d = eventData.pointerDrag.GetComponent<ItemPrefab>();
            if (d != null)
            {
                if (d.item.type == type)
                {
                    d.placeholderParent = this.transform;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            if (eventData.pointerDrag == null)
                return;

            ItemPrefab d = eventData.pointerDrag.GetComponent<ItemPrefab>();
            if (d != null && d.placeholderParent == this.transform)
            {
                d.placeholderParent = d.parentToReturnTo;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

            ItemPrefab d = eventData.pointerDrag.GetComponent<ItemPrefab>();
            if (d != null)
            {
                if (d.item.type == type)
                {
                    d.parentToReturnTo = this.transform;
                    if (contains != null )
                    {
                        contains.UnequipItem();
                    }
                    d.EquipItem();
                    contains = d;                    
                }
            }
        }
    }
}
