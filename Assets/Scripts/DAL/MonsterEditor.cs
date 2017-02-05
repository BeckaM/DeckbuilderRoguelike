using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class MonsterEditor : MonoBehaviour
    {
        public EnemyWrapper MonstersToEdit;

        public void GetMonstersToEdit()
        {
            MonstersToEdit = ObjectDAL.GetAllEnemies();
        }

        public void SaveMonsters()
        {
            ObjectDAL.SaveEnemies(MonstersToEdit);

        }
    }
}
