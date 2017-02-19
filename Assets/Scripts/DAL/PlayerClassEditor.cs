using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class PlayerClassEditor : MonoBehaviour
    {
        public PlayerClassWrapper ClassesToEdit;

        public void GetClassesToEdit()
        {
            ClassesToEdit = ObjectDAL.GetAllClasses();
        }

        public void SaveClasses()
        {
            ObjectDAL.SaveClasses(ClassesToEdit);

        }
    }
}
