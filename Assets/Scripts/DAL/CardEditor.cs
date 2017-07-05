using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class CardEditor : MonoBehaviour
    {
        public CardWrapper CardsToEdit;

        public void GetCardsToEdit()
        {
            CardsToEdit = ObjectDAL.GetAllCards();
        }

        public void SaveCards()
        {
            ObjectDAL.SaveCards(CardsToEdit);
        }
    }
}
