using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTime : MonoBehaviour
{
    [SerializeField] float deathInSeconds;
	void Start ()
    {
        Invoke("die", deathInSeconds);
	}

    public void die()
    {
        Destroy(gameObject);
    }
	
}
