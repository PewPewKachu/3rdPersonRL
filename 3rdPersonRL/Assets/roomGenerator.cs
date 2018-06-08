using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomGenerator : MonoBehaviour
{
    [SerializeField] bool generate;
    [SerializeField] int maxRooms, minRooms;
    GameObject[] rooms;
    GameObject parent;
    bool left, right, forward, backward;

	void Start ()
    {
        rooms = new GameObject[maxRooms];
        rooms[0] = this.gameObject;
	}

    private void FixedUpdate()
    {
        if(generate)
        {
            generate = false;
            int tempNum = Random.Range(minRooms, maxRooms);
            for (int i = 0; i < tempNum; i++)
            {
                int index = nextEmptyIndex();
                if (!rooms[index - 1].GetComponent<roomGenerator>().GetLeft() && Random.Range(0, 3) == 0)
                {
                    left = true;
                    rooms[index] = Instantiate(this.gameObject, rooms[index - 1].transform.position - new Vector3(GetComponent<BoxCollider>().size.x, 0, 0), transform.rotation);
                    rooms[index].GetComponent<roomGenerator>().SetRight(true);
                }
                index = nextEmptyIndex();
                if (!rooms[index - 1].GetComponent<roomGenerator>().GetRight() && Random.Range(0, 3) == 0)
                {
                    right = true;
                    rooms[index] = Instantiate(this.gameObject, rooms[index - 1].transform.position + new Vector3(GetComponent<BoxCollider>().size.x, 0, 0), transform.rotation);
                    rooms[index].GetComponent<roomGenerator>().SetLeft(true);
                }
                index = nextEmptyIndex();
                if (!rooms[index - 1].GetComponent<roomGenerator>().GetForward() && Random.Range(0, 3) == 0)
                {
                    forward = true;
                    rooms[index] = Instantiate(this.gameObject, rooms[index - 1].transform.position + new Vector3(0, 0, GetComponent<BoxCollider>().size.z), transform.rotation);
                    rooms[index].GetComponent<roomGenerator>().SetBackward(true);
                }
                index = nextEmptyIndex();
                if (!rooms[index - 1].GetComponent<roomGenerator>().GetBackward() && Random.Range(0, 3) == 0)
                {
                    backward = true;
                    rooms[index] = Instantiate(this.gameObject, rooms[index - 1].transform.position - new Vector3(0, 0, GetComponent<BoxCollider>().size.z), transform.rotation);
                    rooms[index].GetComponent<roomGenerator>().SetForward(true);
                }
            }
        }

        if (nextEmptyIndex() != rooms.Length
            && nextEmptyIndex() != 1)
            generate = true;
    }

    public int nextEmptyIndex()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] == null)
                return i;
        }
        return rooms.Length;
    }

    public void Generate()
    {
        generate = true;
    }
    
    public void SetLeft(bool _flip)
    {
        left = _flip;
    }
    public void SetRight(bool _flip)
    {
        right = _flip;
    }
    public void SetForward(bool _flip)
    {
        forward = _flip;
    }
    public void SetBackward(bool _flip)
    {
        backward = _flip;
    }

    public bool GetLeft()
    {
        return left;
    }
    public bool GetRight()
    {
        return right;
    }
    public bool GetForward()
    {
        return forward;
    }
    public bool GetBackward()
    {
        return backward;
    }

}
