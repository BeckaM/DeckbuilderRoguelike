using UnityEngine;
using System.Collections;

public class walktest : MonoBehaviour
{
    public Animator anim;
   // public float speed = 5.0f;
    public float rotationSpeed = 0.2f;

   // public Vector3 velocity;
    //public bool right = false;
    public float moveSpeed = 7.0f;
    //new Rigidbody rigidbody;
  //  CharacterController CharCtrl;
   // public Vector3 NextDir;
    public Vector3 movement;

    // Use this for initialization
    void Start()
    {       

       // anim = GetComponent<Animator>();
    }

    void Update()
    {

        ControllPlayer();

    }


    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //transform.rotation = Quaternion.LookRotation(movement);
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);

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



        //if (Input.GetButtonDown("Fire1"))
        //{
        //    animation.Play("attack-01");
        //}
    }

    //void FixedUpdate()
    //{       
    //        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        
    //}
}