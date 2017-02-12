using UnityEngine;
using System.Collections;
using UnityEngine.UI;   //Allows us to use UI.
using UnityEngine.SceneManagement;
using System;

namespace Assets.Scripts
{
    //Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MovingObject
    {


        public int life;
        public int maxLife;

        public PlayerClass playerClass;
        public Sprite playerImage;
        public Sprite[] sprites;

        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.

        public bool moveComplete;
        public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
        public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.
        public AudioClip gameOverSound;             //Audio clip to play when player dies.

        private Animator animator;                  //Used to store a reference to the Player's animator component.
                                 
        private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.

        

        //Start overrides the Start function of MovingObject
        protected override void Start()
        {
            //Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();

            //Set the player properties.
            PopulatePlayer();
            UpdateLife();

            //Call the Start function of the MovingObject base class.
            base.Start();
        }

        public void PopulatePlayer()
        {
            life = GameManager.instance.lifeHolder;
            maxLife = GameManager.instance.maxLife;
            playerClass = GameManager.instance.playerClass;
            playerImage = sprites[playerClass.SpriteIcon];

            var imageObj = transform.GetChild(0);
            var imageComponent = imageObj.GetComponent<SpriteRenderer>();
            imageComponent.sprite = playerImage;

            
        }

        public void LoseLife(int loss)
        {

            //Subtract lost life points from the players total.
            life -= loss;
            CardgameManager.instance.playerLifeText.text = "-" + loss;
            Invoke("UpdateLife", 1f);


        }

        //CheckIfGameOver checks if the player is out of food points and if so, ends the game.

        public void UpdateLife()
        {
            CardgameManager.instance.playerLifeText.text = "Life: " + maxLife + "/" + life;
            GameManager.instance.lifeTextBoard.text = "Life: " + maxLife + "/" + life;

        }






        private void Update()
        {
            //If it's not the player's turn, exit the function.
            //  if (!GameManager.instance.playersTurn) return;
            if (moveComplete || GameManager.instance.doingSetup) return;

            int horizontal = 0;     //Used to store the horizontal move direction.
            int vertical = 0;       //Used to store the vertical move direction.

            //Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            //Check if moving horizontally, if so set vertical to zero.
            if (horizontal != 0)
            {
                vertical = 0;
            }
            //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif //End of mobile platform dependendent compilation section started above with #elif
            //Check if we have a non-zero value for horizontal or vertical
            if (horizontal != 0 || vertical != 0)
            {
               
                //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                AttemptMove(horizontal, vertical);
            }
        }

            //AttemptMove overrides the AttemptMove function in the base class MovingObject
            protected override void AttemptMove(int xDir, int yDir)
        {
           
        
            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove(xDir, yDir);
                        
            //If Move returns true, meaning Player was able to move into an empty space.
            if (Move(xDir, yDir))
            {
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            }



            // Set the playersTurn boolean of GameManager to false now that players turn is over.
            // GameManager.instance.playersTurn = false;
            moveComplete = true;
            StartCoroutine(MoveWait());
            
        }

        IEnumerator MoveWait()
        {
            
            yield return new WaitForSeconds(0.2f);
            moveComplete = false;
           
        }

        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
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
            SceneManager.LoadScene("Main");
        }


        
      


       
    }

}
