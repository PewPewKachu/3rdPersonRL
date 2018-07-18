using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.Cameras;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 15;
    CharacterController myCC;
    float yVel;
    [SerializeField]
    float timeInAir;
    [SerializeField]
    int numJumps;


    // Use this for initialization
    void Start()
    {
        myCC = GetComponent<CharacterController>();
        yVel = -1;
        timeInAir = 0;
        numJumps = 2;
    }

    // Update is called once per frame
    void Update()
    {

        //Get Axis Input
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Check if player wants to jump - if so try to jump
        JumpCheck();

        

        yVel -= 25f * timeInAir * Time.fixedDeltaTime;


        myCC.Move(transform.forward * speed * vAxis * Time.deltaTime);
        myCC.Move(transform.right * speed * hAxis * Time.deltaTime);
        myCC.Move(transform.up * yVel * Time.deltaTime);

        
    }

    private void JumpCheck()
    {

        //if the player is on the ground
        if (myCC.isGrounded)
        {
            yVel = -1; //keeps the player on the ground
            timeInAir = 0;
            numJumps = 2;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVel = 15f;
                numJumps -= 1;
            }
        }
        else // if the player is in the air
        {
            if (Input.GetKeyDown(KeyCode.Space) && numJumps > 0)
            {
                yVel = 15f;
                numJumps -= 1;
                timeInAir = 0;
            }
            timeInAir += Time.fixedDeltaTime * 3f;
        }
    }
}
