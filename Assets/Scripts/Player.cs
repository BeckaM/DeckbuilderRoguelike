using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public int life=30;
        public int maxLife=30;
        public int ward = 0;
        public int damageBoost = 0;

        public int mana = 1;
        public int maxMana = 1;

        public float zComp = 3.5f;
        public float xComp = -0.3f;
        public PlayerClass playerClass;
        public Sprite playerImage;
        public Sprite[] sprites;
        new Rigidbody rigidbody;
        public Vector3 velocity;
        public bool right = false;
        public GameObject playerBody;
        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
        public float moveSpeed= 7.0f;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

            if (Input.GetAxisRaw("Horizontal") > 0 && !right)
            {
                playerBody.transform.Rotate(0, 180, 0);
                right = true;
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && right)
            {
                playerBody.transform.Rotate(0, 180, 0);
                right = false;
            }

            if (Input.GetMouseButton(0) && GameManager.instance.doingSetup == false)
            {
                var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPos.y = transform.position.y;
                targetPos.x += xComp;
                targetPos.z += zComp;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            if (GameManager.instance.doingSetup == false)
            {
                rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
            }
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
                GameManager.instance.doingSetup = true;
            }
            else if (other.tag == "Chest")
            {
                other.GetComponent<ChestManager>().OpenChest();
                GameManager.instance.doingSetup = true;
            }
            else if (other.tag == "Shrine")
            {
                other.GetComponent<ShrineManager>().OpenShrine();
                GameManager.instance.doingSetup = true;
            }
            else if (other.tag == "Anvil")
            {
                other.GetComponent<AnvilManager>().OpenAnvil();
                GameManager.instance.doingSetup = true;
            }
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game.
            SceneManager.LoadScene("Scene 3D");
        }
    }
}