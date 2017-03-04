using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Player3d : MonoBehaviour
    {
        public int life;
        public int maxLife;
        public int ward = 0;

        public int mana = 1;
        public int maxMana = 1;

        public PlayerClass playerClass;
        public Sprite playerImage;
        public Sprite[] sprites;
        new Rigidbody rigidbody;
        public Vector3 velocity;
        public bool right = false;
        public GameObject playerimage;
        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * 10;


            if (Input.GetAxisRaw("Horizontal") > 0 && !right)
            {
                playerimage.transform.Rotate(0, 180, 0);
                right = true;
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && right)
            {
                playerimage.transform.Rotate(0, 180, 0);
                right = false;
            }


        }

        void FixedUpdate()
        {
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }

        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter(Collider other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
            }
            else if (other.tag == "Enemy")
            {
                GameManager.instance.InitCardgame(other, this);
            }
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Store life in Gamemanager between scenes
            GameManager.instance.lifeHolder = life;
            //Load the last scene loaded, in this case Main, the only scene in the game.
            SceneManager.LoadScene("Scene 3D");
        }
    }
}