using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomGenerator : MonoBehaviour
{
    [SerializeField] bool generate;
    [SerializeField] int maxRooms, minRooms;
    [SerializeField] GameObject room;
    GameObject[] rooms;
    GameObject parent;
    [SerializeField]bool left, right, forward, backward;

	void Start ()
    {
        rooms = new GameObject[Random.Range(minRooms, maxRooms)];
        rooms[0] = this.gameObject;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}

    private void FixedUpdate()
    {
        if(generate)
        {
            generate = false;
            for (int i = 0; i < rooms.Length; i++)
            {
                int index = nextEmptyIndex() - 1;
                if (!rooms[index].GetComponent<roomGenerator>().GetLeft() && Random.Range(0, 2) == 0 && index < rooms.Length)
                {
                    int TEMP = nextEmptyIndex();
                    if (!checkPosition(rooms[index].transform.position - new Vector3(GetComponent<BoxCollider>().size.x, 0, 0)))
                    {
                        rooms[index].GetComponent<roomGenerator>().SetLeft(true);

                        rooms[TEMP] = Instantiate(room, rooms[index].transform.position - new Vector3(GetComponent<BoxCollider>().size.x, 0, 0), transform.rotation);

                        rooms[TEMP].GetComponent<roomGenerator>().SetRight(true);
                    }
                }
                if (!rooms[index].GetComponent<roomGenerator>().GetRight() && Random.Range(0, 2) == 0 && index < rooms.Length)
                {
                    int TEMP = nextEmptyIndex();
                    if (!checkPosition(rooms[index].transform.position + new Vector3(GetComponent<BoxCollider>().size.x, 0, 0)))
                    {
                        rooms[index].GetComponent<roomGenerator>().SetRight(true);

                        rooms[TEMP] = Instantiate(room, rooms[index].transform.position + new Vector3(GetComponent<BoxCollider>().size.x, 0, 0), transform.rotation);

                        rooms[TEMP].GetComponent<roomGenerator>().SetLeft(true);
                    }
                }
                if (!rooms[index].GetComponent<roomGenerator>().GetForward() && Random.Range(0, 2) == 0 && index < rooms.Length)
                {
                    int TEMP = nextEmptyIndex();
                    if (!checkPosition(rooms[index].transform.position + new Vector3(0, 0, GetComponent<BoxCollider>().size.z)))
                    {
                        rooms[index].GetComponent<roomGenerator>().SetForward(true);

                        rooms[TEMP] = Instantiate(room, rooms[index].transform.position + new Vector3(0, 0, GetComponent<BoxCollider>().size.z), transform.rotation);
                       

                        rooms[TEMP].GetComponent<roomGenerator>().SetBackward(true);
                    }
                }
                if (!rooms[index].GetComponent<roomGenerator>().GetBackward() && Random.Range(0, 2) == 0 && index < rooms.Length)
                {
                    int TEMP = nextEmptyIndex();
                    if (!checkPosition(rooms[index].transform.position - new Vector3(0, 0, GetComponent<BoxCollider>().size.z)))
                    {
                        rooms[index].GetComponent<roomGenerator>().SetBackward(true);

                        rooms[TEMP] = Instantiate(room, rooms[index].transform.position - new Vector3(0, 0, GetComponent<BoxCollider>().size.z), transform.rotation);

                        rooms[TEMP].GetComponent<roomGenerator>().SetForward(true);
                    }
                }
            }
        }
        
    }

    public int nextEmptyIndex()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] == null)
                return i;
        }
        return rooms.Length - 1;
    }

    public bool checkEmpty()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] == null)
                return false;
        }
        return true;
    }

    public bool checkPosition(Vector3 pos)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] != null && rooms[i].transform.position == pos)
                return true;
        }
        return false;
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
