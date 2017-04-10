using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts
{

    public class UnlockObject : MonoBehaviour
    {      
               
        public TMP_Text unlockText;
        public TMP_Text metricText;

        public void PopulateUnlock(PlayerClass unlock)
        {
            unlockText.text = "New Class unlocked: " + unlock.ClassName;
            metricText.text = unlock.conditionText;
        }


        public void PopulateUnlock(Perk unlock)
        {
            unlockText.text = "New Perk unlocked: " + unlock.perkName;
            metricText.text = unlock.conditionText;
        }
    }
}