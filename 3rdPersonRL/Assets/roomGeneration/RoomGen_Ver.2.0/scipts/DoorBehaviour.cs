using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    enum DoorState { openInner, closed, openOuter }

    [SerializeField] DoorState curState;
    [SerializeField] float moveSpeed, kickForce;

    [SerializeField] GameObject smoke;

    Quaternion closed, openInner, openOuter;
    GameObject player;

    Rigidbody rigid;
    Animator anim;

    bool broken = false;
    Vector3 rotate, origin;

	void Start ()
    {
        closed = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        openInner = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + 90, 0));
        openOuter = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y - 90, 0));

        rotate = new Vector3(1, 0, 0);

        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        origin = transform.position;
    }
	

	void Update ()
    {
        switch (curState)
        {
            case DoorState.openInner:
                if(!broken)
                    transform.rotation = Quaternion.Lerp(transform.rotation, openInner, Time.deltaTime * moveSpeed);
                break;
            case DoorState.closed:
                if(!broken)
                    transform.rotation = Quaternion.Lerp(transform.rotation, closed, Time.deltaTime * moveSpeed);
                break;
            case DoorState.openOuter:
                if(!broken)
                    transform.rotation = Quaternion.Lerp(transform.rotation, openOuter, Time.deltaTime * moveSpeed);
                break;
            default:
                break;
        }
        if(!broken)
            collisionCheck();
    }
    public void collisionCheck()
    {
        transform.position = origin;
        if (curState == DoorState.closed && Vector3.Distance(transform.position, player.transform.position) <= 3f  && Input.GetKeyDown(KeyCode.F))
        {
            broken = true;
            Vector3 dir = (transform.position - player.transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);

            Instantiate(smoke, transform.position, transform.rotation);

            rigid.useGravity = true;
            rigid.constraints = RigidbodyConstraints.None;
            if (dot > 0)
            {
                rigid.AddForce(transform.forward * kickForce);
                transform.Rotate(rotate * kickForce);
            }
            else if (dot <= 0)
            {
                rigid.AddForce(-transform.forward * kickForce);
                transform.Rotate(-rotate * kickForce);
            }
        }
        else if (curState == DoorState.closed && Vector3.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);

            if (dot > 0)
                curState = DoorState.openInner;
            else if (dot <= 0)
                curState = DoorState.openOuter;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) <= 3f && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);
            if (curState != DoorState.closed)
                curState = DoorState.closed;
            else if (dot > 0)
                curState = DoorState.openInner;
            else if (dot <= 0)
                curState = DoorState.openOuter;
        }
    }
}
