using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float speed = 150;
    Rigidbody myRb;
	// Use this for initialization
	void Start ()
    {
        myRb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        
        myRb.AddForce(transform.forward * speed * vAxis);
        myRb.AddForce(transform.right * speed * hAxis);

    }
}
