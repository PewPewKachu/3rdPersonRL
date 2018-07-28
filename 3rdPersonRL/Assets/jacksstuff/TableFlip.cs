using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFlip : MonoBehaviour
{
    [SerializeField] bool flip;
    [SerializeField] float rotX, rotY, rotZ, detectionDistance;

    bool once;

    Vector3 rotation;
    GameObject[] player;

	void Start ()
    {
        rotation = new Vector3(rotX, rotY, rotZ);
        player = GameObject.FindGameObjectsWithTag("Player");
	}

    private void FixedUpdate()
    {
        if(flip)
        {
            flip = false;
            Flip();
        }
        if (!once)
        {
            for (int i = 0; i < player.Length; i++)
            {
                if (Input.GetKeyDown("e") && Vector3.Distance(transform.position, player[i].transform.position) < detectionDistance)
                {
                    flip = true;
                    once = true;
                }
            }
        }
    }

    public void Flip()
    {
        transform.Rotate(rotation);
    }
}
