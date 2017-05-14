using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public Animator anim;
        public bool moveToggle = false;

        public float rotationSpeed = 0.2f;
        public float moveSpeed = 7.0f;
        public float deadZone = 0.5f;

        public Vector3 movement;

        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
        public float zComp = 3.5f;
        public float xComp = -0.3f;

        private void OnEnable()
        {
            moveToggle = false;
            anim.SetBool("IsWalking", false);
        }

        void Update()
        {
            GetKeyboardInput();
            GetMouseInput();
            MovePlayer();
        }


        private void GetMouseInput()
        {
            if (moveToggle && !GameManager.instance.doingSetup)  //moveToggle is set by clicking on dungeon walls or ground.
            {
                //Debug.Log("X position: " + Input.mousePosition.x);
                //Debug.Log("Y position: " + Input.mousePosition.y);

                var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPos.y = transform.position.y;
                targetPos.x += xComp;
                targetPos.z += zComp;               

                var heading = targetPos - transform.position;
                var distance = heading.magnitude;
                if (distance < deadZone)            // Deadzone around the player to remove twitchyness. 
                {
                    return;
                }
                var direction = heading / distance; // This is now the normalized direction.
                movement = direction;

            }
        }
        
        private void GetKeyboardInput()
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        }

        
        void MovePlayer()
        {
            if(!GameManager.instance.doingSetup)
            {
                if (movement != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);

                transform.Translate(Vector3.ClampMagnitude(movement, 1.0f) * moveSpeed * Time.deltaTime, Space.World);

                if (movement != Vector3.zero)
                {
                    anim.SetBool("IsWalking", true);
                }
                else
                {
                    anim.SetBool("IsWalking", false);
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);
                
            }
            else if (other.tag == "Enemy")
            {
                GameManager.instance.InitCardgame(other);
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
            SceneManager.LoadScene("Main");
        }



        //void FixedUpdate()
        //{       
        //        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        //}
    }
}