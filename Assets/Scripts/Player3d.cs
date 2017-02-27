using UnityEngine;
using System.Collections;

public class Player3d : MonoBehaviour {

	Rigidbody rigidbody;
	public Vector3 velocity;
    public bool right = false;
    public GameObject playerimage;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
        

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

	void FixedUpdate() {
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
