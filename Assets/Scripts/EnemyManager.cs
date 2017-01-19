using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

    private const string fileName = @"C:\Users\Public\Documents\Unity Projects\DeckbuilderRoguelike\Assets\JSON\Enemies.json";


    public string EnemyName;
    public int SpriteIcon;
    public int Level;
    public int HP;
    public List<string> EnemyDeck;

    public GameObject EnemyObject;
    public Sprite[] sprites;


    public void CreateEnemy()
    {
        string text = File.ReadAllText(fileName);
        var enemylist = JsonUtility.FromJson<EnemyWrapper>(text);

        Enemy enemy = new Enemy
        {
            EnemyName = this.EnemyName,
            SpriteIcon = this.SpriteIcon,
            Level = this.Level,
            HP = this.HP,
            EnemyDeck = this.EnemyDeck
        };

        if (enemylist == null)
        {
            enemylist = new EnemyWrapper();
            enemylist.EnemyItems = new System.Collections.Generic.List<Enemy>();
        }

        enemylist.EnemyItems.Add(enemy);


        string jsonEnemy = JsonUtility.ToJson(enemylist);
        SaveEnemy(jsonEnemy);
    }




    private void SaveEnemy(string awesomeNewMonster)
    {
        if (!File.Exists(fileName))
        {
            return;
        }

        File.WriteAllText(fileName, awesomeNewMonster);
    }
    public void GetEnemy(Enemy enemy)
    {

        EnemyName = enemy.EnemyName;
        SpriteIcon = enemy.SpriteIcon;
        Level = enemy.Level;
        HP = enemy.HP;
        EnemyDeck = enemy.EnemyDeck;

    var transformer = this.transform;

        //Set Image
        var imageObj = transformer.GetChild(0);
        var imageComponent = imageObj.GetComponent<Image>();
        imageComponent.sprite = sprites[enemy.SpriteIcon];





    }

  






    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
