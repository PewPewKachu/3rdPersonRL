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
                    Destroy(gameObject);
                break;
            case wall.backward:
                if (generator.GetBackward())
                    Destroy(gameObject);
                break;
            case wall.right:
                if (generator.GetRight())
                    Destroy(gameObject);
                break;
            case wall.left:
                if (generator.GetLeft())
                    Destroy(gameObject);
                break;
        }
	}

    private void Update()
    {
        switch (direction)
        {
            case wall.forward:
                if (generator.GetForward())
                    Destroy(gameObject);
                break;
            case wall.backward:
                if (generator.GetBackward())
                    Destroy(gameObject);
                break;
            case wall.right:
                if (generator.GetRight())
                    Destroy(gameObject);
                break;
            case wall.left:
                if (generator.GetLeft())
                    Destroy(gameObject);
                break;
        }
    }
}
