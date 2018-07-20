using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    enum DoorState { openInner, closed, openOuter }

    [SerializeField] DoorState curState;
    [SerializeField] float moveSpeed, kickForce;

    Quaternion closed, openInner, openOuter;
    GameObject player;

	void Start ()
    {
        closed = Quaternion.Euler(new Vector3(0, 0, 0));
        openInner = Quaternion.Euler(new Vector3(0, 90, 0));
        openOuter = Quaternion.Euler(new Vector3(0, -90, 0));

        player = GameObject.FindGameObjectWithTag("Player");
    }
	

	void Update ()
    {
        switch (curState)
        {
            case DoorState.openInner:
                transform.rotation = Quaternion.Lerp(transform.rotation, openInner, Time.deltaTime * moveSpeed);
                break;
            case DoorState.closed:
                transform.rotation = Quaternion.Lerp(transform.rotation, closed, Time.deltaTime * moveSpeed);
                break;
            case DoorState.openOuter:
                transform.rotation = Quaternion.Lerp(transform.rotation, openOuter, Time.deltaTime * moveSpeed);
                break;
            default:
                break;
        }

        if(Vector3.Distance(transform.position, player.transform.position) <= 1.5f && curState == DoorState.closed)
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);

            if (dot > 0)
                curState = DoorState.openInner;
            else if (dot <= 0)
                curState = DoorState.openOuter;
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= 3f && Input.GetKeyDown(KeyCode.E))
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" && curState == DoorState.closed)
        {
            Debug.Log("whats up");
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);

            if (dot < 0)
                curState = DoorState.openInner;
            else if (dot >= 0)
                    curState = DoorState.openOuter;
        }
    }
}
