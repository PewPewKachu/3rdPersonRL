using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 15;
    CharacterController myCC;
    float yVel;
    [SerializeField]
    float timeInAir;
    [SerializeField]
    int numJumps;
    [SerializeField]
    bool isGrounded;
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
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        isGrounded = myCC.isGrounded;

        if (myCC.isGrounded)
        {
            yVel = -1;
            timeInAir = 0;
            numJumps = 2;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVel = 15f;
                numJumps -= 1;
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && numJumps > 0)
            {
                yVel = 15f;
                numJumps -= 1;
                timeInAir = 0;
            }
            timeInAir += Time.fixedDeltaTime * 3f;
        }

        yVel -= 25f * timeInAir * Time.fixedDeltaTime;


        myCC.Move(transform.forward * speed * vAxis * Time.deltaTime);
        myCC.Move(transform.right * speed * hAxis * Time.deltaTime);
        myCC.Move(transform.up * yVel * Time.deltaTime);

    }
}
