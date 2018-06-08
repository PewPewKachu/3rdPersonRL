using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallscript : MonoBehaviour
{
    enum wall { left, right, forward, backward };
    [SerializeField] wall direction;
    roomGenerator generator;
    
	void Start ()
    {
        generator = GetComponentInParent<roomGenerator>();

        switch(direction)
        {
            case wall.forward:
                if (generator.GetForward())
                    gameObject.SetActive(false);
                break;
            case wall.backward:
                if (generator.GetBackward())
                    gameObject.SetActive(false);
                break;
            case wall.right:
                if (generator.GetRight())
                    gameObject.SetActive(false);
                break;
            case wall.left:
                if (generator.GetLeft())
                    gameObject.SetActive(false);
                break;
        }
	}

    private void Update()
    {
        switch (direction)
        {
            case wall.forward:
                if (generator.GetForward())
                    gameObject.SetActive(false);
                break;
            case wall.backward:
                if (generator.GetBackward())
                    gameObject.SetActive(false);
                break;
            case wall.right:
                if (generator.GetRight())
                    gameObject.SetActive(false);
                break;
            case wall.left:
                if (generator.GetLeft())
                    gameObject.SetActive(false);
                break;
        }
    }
}
