using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace Assets.Scripts
{

    public class CardgameUI : MonoBehaviour
    {
        public Image playerPortrait;
        public TMP_Text playerLifeText;
        public TMP_Text playerManaText;
        public TMP_Text playerDeckCount;
        public TMP_Text playerDiscardCount;

        public Image monsterPortrait;
        public TMP_Text monsterLifeText;
        public TMP_Text monsterManaText;
        public TMP_Text monsterDeckCount;
        public TMP_Text monsterDiscardCount;

        public Button endTurnButton;
        public GameObject muliganPanel;
        public MuliganPanel muliganPanelScript;

        void OnEnable()
        {
            EventManager.Instance.AddListener<UpdateMana_GUI>(GUIUpdateMana);
            EventManager.Instance.AddListener<UpdateLife_GUI>(GUIUpdateLife);
            EventManager.Instance.AddListener<ApplyDamage_GUI>(ApplyDamageGUI);
            EventManager.Instance.AddListener<UpdateDeckTexts_GUI>(GUIUpdateDeckDiscardText);
            EventManager.Instance.AddListener<EndGame_GUI>(EndGame);
        }

        void OnDisable()
        {
            EventManager.Instance.RemoveAll();
        }

        private void GUIUpdateDeckDiscardText(UpdateDeckTexts_GUI updates)
        {
            if (updates.team == CardgameManager.Team.Me)
            {
                playerDeckCount.text = updates.decktext.ToString();
                playerDiscardCount.text = updates.discardtext.ToString();
            }
            else
            {
                monsterDeckCount.text = updates.decktext.ToString();
                monsterDiscardCount.text = updates.discardtext.ToString();
            }
            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateMana(UpdateMana_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerManaText.text = "Mana:" + e.mana + "/" + e.maxMana;
            }
            else
            {
                monsterManaText.text = "Mana:" + e.mana + "/" + e.maxMana;
            }

            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateLife(UpdateLife_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerLifeText.text = "Life:" + e.life + "/" + e.maxLife;
            }
            else
            {
                monsterLifeText.text = "Life:" + e.life + "/" + e.maxLife;
            }

            EventManager.Instance.processingQueue = false;
        }


        internal void ApplyDamageGUI(ApplyDamage_GUI a)
        {
            var damageText = "" + a.damage + " Damage!";
            var reduceText = "" + a.reduced + " Reduced";
            var effectText = "";
            if (a.reduced > 0)
            {
                effectText = damageText + " " + reduceText;
            }
            else
            {
                effectText = damageText;
            }

            if (a.team == CardgameManager.Team.Me)
            {
                StartCoroutine(EffectText(Color.red, playerLifeText, effectText, 3));
            }
            else
            {
                StartCoroutine(EffectText(Color.red, monsterLifeText, effectText, 3));
            }
        }


        private IEnumerator EffectText(Color color, TMP_Text textObject, string text, int loops)
        {
           // var effectText = textObject.GetComponent<Text>();
            textObject.text = text;
            textObject.color = color;

            for (var n = 0; n < loops; n++)
            {
                textObject.color = Color.white;
                yield return new WaitForSeconds(.1f);
                textObject.color = color;
                yield return new WaitForSeconds(.1f);
            }
            EventManager.Instance.processingQueue = false;
        }


        private void EndGame(EndGame_GUI end)
        {
            DeckManager.player.Cleanup();
            DeckManager.monster.Cleanup();

            GameManager.instance.ReturnFromCardgame(end.playerWon, end.cardReward, end.goldReward);

            EventManager.Instance.processingQueue = false;

            this.gameObject.SetActive(false);
        }
    }
}