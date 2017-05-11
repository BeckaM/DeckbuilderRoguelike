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
        public TMP_Text playerArmorText;
        public TMP_Text playerDamageIncreaseText;
        // public TMP_Text playerDeckCount;
        // public TMP_Text playerDiscardCount;

        public Image monsterPortrait;
        public TMP_Text monsterNameText;
        public TMP_Text monsterLifeText;
        public TMP_Text monsterManaText;
        public TMP_Text monsterArmorText;
        public TMP_Text monsterDamageIncreaseText;
        // public TMP_Text monsterDeckCount;
        // public TMP_Text monsterDiscardCount;

        public Button endTurnButton;
        public GameObject muliganPanel;
        public MuliganPanel muliganPanelScript;

        void OnEnable()
        {
            EventManager.Instance.AddListener<UpdateMana_GUI>(GUIUpdateMana);
            EventManager.Instance.AddListener<UpdateLife_GUI>(GUIUpdateLife);
            EventManager.Instance.AddListener<ApplyDamage_GUI>(ApplyDamageGUI);
            EventManager.Instance.AddListener<UpdateArmor_GUI>(GUIUpdateArmor);
            EventManager.Instance.AddListener<UpdateDamageIncrease_GUI>(GUIUpdateDamageIncrease);
            //   EventManager.Instance.AddListener<UpdateDeckTexts_GUI>(GUIUpdateDeckDiscardText);
            EventManager.Instance.AddListener<EndGame_GUI>(EndGame);
        }

        public void ResetUI(Player player, EnemyManager enemy)
        {
            playerManaText.text = player.mana.ToString() + "/" + player.maxMana.ToString();
            monsterManaText.text = enemy.mana.ToString() + "/" + enemy.maxMana.ToString();

            playerLifeText.text = player.life.ToString() + "/" + player.maxLife.ToString();
            monsterLifeText.text = enemy.life.ToString() + "/" + enemy.maxLife.ToString();

            playerArmorText.text = player.ward.ToString();
            monsterArmorText.text = enemy.ward.ToString();

            playerDamageIncreaseText.text = player.damageBoost.ToString();
            monsterDamageIncreaseText.text = enemy.damageBoost.ToString();
        }

        void OnDisable()
        {
            EventManager.Instance.RemoveAll();
        }

        //private void GUIUpdateDeckDiscardText(UpdateDeckTexts_GUI updates)
        //{
        //    if (updates.team == CardgameManager.Team.Me)
        //    {
        //        playerDeckCount.text = updates.decktext.ToString();
        //        playerDiscardCount.text = updates.discardtext.ToString();
        //    }
        //    else
        //    {
        //        monsterDeckCount.text = updates.decktext.ToString();
        //        monsterDiscardCount.text = updates.discardtext.ToString();
        //    }
        //    EventManager.Instance.processingQueue = false;
        //}

        private void GUIUpdateMana(UpdateMana_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerManaText.text = e.mana.ToString() + "/" + e.maxMana.ToString();
            }
            else
            {
                monsterManaText.text = e.mana.ToString() + "/" + e.maxMana.ToString();
            }

            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateLife(UpdateLife_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerLifeText.text = e.life.ToString() + "/" + e.maxLife.ToString();
            }
            else
            {
                monsterLifeText.text = e.life.ToString() + "/" + e.maxLife.ToString();
            }

            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateArmor(UpdateArmor_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerArmorText.text = e.armor.ToString();
            }
            else
            {
                monsterArmorText.text = e.armor.ToString();
            }

            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateDamageIncrease(UpdateDamageIncrease_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == CardgameManager.Team.Me)
            {
                playerDamageIncreaseText.text = e.boost.ToString();
            }
            else
            {
                monsterDamageIncreaseText.text = e.boost.ToString();
            }

            EventManager.Instance.processingQueue = false;
        }


        internal void ApplyDamageGUI(ApplyDamage_GUI a)
        {

            var effectText = "-" + a.damage.ToString();
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
            CardgameManager.instance.PlayerCleanup();

            GameManager.instance.ReturnFromCardgame(end.playerWon, end.cardRewards, end.goldReward);

            EventManager.Instance.processingQueue = false;

            this.gameObject.SetActive(false);
            GameManager.instance.playerObject.SetActive(true);
            CardgameManager.instance.gameObject.SetActive(false);

        }
    }
}