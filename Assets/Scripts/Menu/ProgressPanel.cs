using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class ProgressPanel : MonoBehaviour
    {
        public Menu mainMenu;

        public TMP_Text monsterKills;

        public GameObject classPanel;
        public TMP_Text monsterKillsClass;

        public Button closeButton;      

        public void ShowProgress()
        {
            var progress = GameManager.instance.progressManager.totalProgress;

            monsterKills.text = "Monster kills: " + progress.monsterKills.ToString();
           
        }

        public void ShowClassProgress(PlayerClass selectedClass)
        {
            var progress = GameManager.instance.progressManager.totalProgress;
            var currentClass = progress.classProgressList.Find(item => item.className.Equals(selectedClass.ClassName));
            classPanel.SetActive(true);

            monsterKillsClass.text = currentClass.monsterKills.ToString();

        }

        
    }
}
