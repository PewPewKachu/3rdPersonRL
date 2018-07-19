using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    enum DoorState { openInner, closed, openOuter }

    [SerializeField] DoorState curState;
    [SerializeField] float moveSpeed;

    Quaternion closed, openInner, openOuter;

	void Start ()
    {
        closed = Quaternion.Euler(new Vector3(0, 0, 0));
        openInner = Quaternion.Euler(new Vector3(0, 90, 0));
        openOuter = Quaternion.Euler(new Vector3(0, -90, 0));

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" && curState == DoorState.closed)
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
        }
    }
}
