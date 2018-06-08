using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] float speed, jumpforce;
    Rigidbody rigid, thingRigid;
    bool grounded;
    float hold, forceCount, reallyhold, sizeHold;


    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    void Start ()
    {
        sizeHold = 2;
        rigid = GetComponent<Rigidbody>();
        hold = speed;
        reallyhold = hold;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && grounded)
            hold = reallyhold * 2;
        else
            hold = reallyhold;

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * (speed / 2) * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rigid.velocity += new Vector3(0, jumpforce, 0);
        grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
