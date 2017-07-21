using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Assets.Scripts.Menu
{
    public class ItemPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ItemSelect itemSelect;

        public Image itemImage;
        public List<Sprite> images;        
        public TMP_Text itemName;
        public TMP_Text popupName;
        public TMP_Text popupDescription;

        public Item item;
        
       // public bool equiped;

        public Transform parentToReturnTo = null;
        public Transform placeholderParent = null;

        GameObject placeholder = null;

        public void PopulateItemPrefab(Item item, ItemSelect itemSelect)
        {
            this.itemSelect = itemSelect;
            this.item = item;
            itemName.text = item.itemName;
            itemImage.sprite = images[item.itemSprite];
            popupName.text = item.itemName;
            popupDescription.text = item.itemEffectText;
        }

        public void EquipItem()
        {
            itemSelect.AddItem(this);
        }

        public void UnequipItem()
        {
            itemSelect.RemoveItem(this);
            if(item.type == Item.ItemType.Trinket)
            {
                transform.SetParent(itemSelect.trinketSelectHolder.transform);
            }
            else
            {
                transform.SetParent(itemSelect.weaponSelectHolder.transform);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");

            itemSelect.HighlightSlots(item.type, true);
            UnequipItem();

            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            parentToReturnTo = this.transform.parent;
            placeholderParent = parentToReturnTo;
            this.transform.SetParent(this.transform.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log ("OnDrag");

            this.transform.position = eventData.position;

            if (placeholder.transform.parent != placeholderParent)
                placeholder.transform.SetParent(placeholderParent);

            int newSiblingIndex = placeholderParent.childCount;

            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {

                    newSiblingIndex = i;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;

                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");

            itemSelect.HighlightSlots(item.type, false);

            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            Destroy(placeholder);
        }
    }
}


