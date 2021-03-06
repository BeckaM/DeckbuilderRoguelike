﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PlayerPawn : MonoBehaviour
    {
        public Animator anim;
        public bool moveToggle = false;

        public float rotationSpeed = 0.2f;
        public float moveSpeed = 7.0f;
        public float deadZone = 0.5f;

        public Vector3 targetPos;

        public Vector3 movement;

        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
        [Tooltip("Up/Down correction")]
        public float zComp = 3.5f;
        [Tooltip("Left/Right Correction")]
        public float xComp = -0.3f;

        public List<GameObject> playerModels;
        public GameObject playerModelHolder;

        private void OnEnable()
        {
            moveToggle = false;
            
        }

        void Update()
        {
            GetKeyboardInput();
            GetMouseInput();
            MovePlayer();
        }               

        public void SetPlayerModel(int playerModel)
        {
            var pModel = Instantiate(playerModels[playerModel], playerModelHolder.transform);
            anim = pModel.GetComponent<Animator>();
            anim.SetBool("IsWalking", false);
        }

        private void GetMouseInput()
        {
            if (moveToggle && !GameManager.instance.doingSetup)  //moveToggle is set by clicking on dungeon walls or ground.
            {
                //Debug.Log("X position: " + Input.mousePosition.x);
                //Debug.Log("Y position: " + Input.mousePosition.y);

                //targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //targetPos.y = transform.position.y;
                //targetPos.x += xComp;
                //targetPos.z += zComp;
                //targetPos1 = targetPos;
                               
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    Vector3 newpos = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                    Vector3 dir = (newpos - this.transform.position).normalized;


                    //   var heading = targetPos - transform.position;
                    var distance = dir.magnitude;
                    //if (distance < deadZone)            // Deadzone around the player to remove twitchyness. 
                    //{
                    //    return;
                    //}
                    //var direction = heading / distance; // This is now the normalized direction.
                    movement = dir;
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(targetPos, 1);
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