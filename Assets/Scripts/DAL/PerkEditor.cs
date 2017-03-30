using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class PerkEditor : MonoBehaviour
    {
        public PerkWrapper PerksToEdit;

        public void GetPerksToEdit()
        {
            PerksToEdit = ObjectDAL.GetAllPerks();
        }

        public void SavePerks()
        {
            ObjectDAL.SavePerks(PerksToEdit);

        }
    }
}
