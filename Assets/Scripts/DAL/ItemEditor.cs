using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class ItemEditor : MonoBehaviour
    {
        public ItemWrapper ItemsToEdit;

        public void GetItemsToEdit()
        {
            ItemsToEdit = ObjectDAL.GetAllItems();
        }

        public void SaveItems()
        {
            ObjectDAL.SaveItems(ItemsToEdit);
        }
    }
}
