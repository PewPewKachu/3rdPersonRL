using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGen : MonoBehaviour
{
    [SerializeField] int MaxRooms, MinRooms;
    float numOfRooms;

    [SerializeField] GameObject[] hallways, rooms; //Templates

    GameObject[] GeneratedRooms, GeneratedHalls, HallsHold, RoomsHold; //Data Storage

    NavMeshSurface nav;
	void Start ()
    {
        nav = GameObject.FindGameObjectWithTag("navMesh").GetComponent<NavMeshSurface>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Create Templates
        HallsHold = new GameObject[hallways.Length];
        for (int i = 0; i < hallways.Length; i++)
        {
            HallsHold[i] = Instantiate(hallways[i], transform.position, transform.rotation);
        }
        RoomsHold = new GameObject[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            RoomsHold[i] = Instantiate(rooms[i], transform.position, transform.rotation);
        }

        GeneratedHalls = new GameObject[MaxRooms + 1];
        GeneratedRooms = new GameObject[MaxRooms];

        GeneratedRooms[0] = this.gameObject;

        numOfRooms = Random.Range(MinRooms, MaxRooms); //Get number of rooms

        //GENERATE RANDOM ROOMS//
        for (int i = 1; i < numOfRooms; i++)
        {
            int index = Random.Range(0, i); //Get Random Room
            GeneratedRooms[i] = GenerateRoom(GeneratedRooms[index], i); //Generate from RandRoom

            if (GeneratedRooms[i] == null)
                i--;
        }

        Invoke("BuildNav", 0.25f);

        //Delete Templates
        for (int i = 0; i < RoomsHold.Length; i++)
            Destroy(RoomsHold[i].gameObject);

        for (int i = 0; i < HallsHold.Length; i++)
            Destroy(HallsHold[i].gameObject);
	}

    public void BuildNav()
    {
        nav.BuildNavMesh();
    }

    public GameObject GenerateRoom(GameObject room, int _index)
    {
        int temp2 = 0;
        Transform[] _doors = room.GetComponent<RoomScript>().getDoors();
        GameObject spawnedHall = null;
        //Generate a Hallway
        for (int temp = 0; temp < _doors.Length; temp++)
        {
            int i = Random.Range(0, hallways.Length);
            temp2 = Random.Range(0, _doors.Length);
            GameObject Hall = HallsHold[i];
            RoomScript hallDoors = Hall.GetComponent<RoomScript>();

            //Rotate Template
            Hall.transform.rotation = Quaternion.Euler(Hall.transform.rotation.x,
                RotationCalculator(_doors[temp2].transform.eulerAngles.y, hallDoors.getDoors()[0].eulerAngles.y),
                Hall.transform.rotation.z);
            //Position Template
            Hall.transform.position = _doors[temp2].transform.position + (Hall.transform.position - hallDoors.getDoors()[0].transform.position);


            if (CheckCollision(Hall, room))
            {
                //Generate Hall
                spawnedHall = Instantiate(hallways[i], Hall.transform.position, Hall.transform.rotation);
                //Reset Remplate
                Hall.transform.position = transform.position;
                Hall.transform.rotation = transform.rotation;
                break;
            }
            else
            { 
                //Reset Template
                Hall.transform.position = transform.position;
                Hall.transform.rotation = transform.rotation;
            }
        }

        if (spawnedHall == null) //If no hall return null
            return null;

        //Generate Room
        for (int i = 0; i < rooms.Length; i++)
        {
            int roomTemp = Random.Range(0, rooms.Length);
            GameObject Room = RoomsHold[roomTemp];
            RoomScript roomDoors = Room.GetComponent<RoomScript>();

            for (int j = 0; j < roomDoors.getDoors().Length; j++)
            {
                int jTemp = Random.Range(0, roomDoors.getDoors().Length);
                //Rotate Room
                Room.transform.rotation = Quaternion.Euler(new Vector3(Room.transform.rotation.x, RotationCalculator(spawnedHall.GetComponent<RoomScript>().getDoors()[1].eulerAngles.y
                                        , roomDoors.getDoors()[jTemp].transform.eulerAngles.y)
                                        ,Room.transform.rotation.z));
                //Position Room
                Room.transform.position = spawnedHall.GetComponent<RoomScript>().getDoors()[1].transform.position +
                    (Room.transform.position - roomDoors.getDoors()[jTemp].transform.position);

                if (CheckCollision(Room, spawnedHall))
                {
                    //Generate Room
                    GameObject spawned = Instantiate(rooms[roomTemp], Room.transform.position, Room.transform.rotation);
                    spawned.GetComponent<RoomScript>().setDoor(jTemp); //Set ActiveDoor
                    room.GetComponent<RoomScript>().setDoor(temp2);
                    GeneratedHalls[_index] = spawnedHall;

                    //Reset Template
                    Room.transform.position = transform.position;
                    Room.transform.rotation = transform.rotation;

                    return spawned;
                }

                Room.transform.rotation = transform.rotation;
                Room.transform.position = transform.position;
            }
            Room.transform.rotation = transform.rotation;
            Room.transform.position = transform.position;
        }
        Destroy(spawnedHall);

        return null;
    }

    public int GetNextEmpty(GameObject[] _array)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            if (_array[i] == null)
                return i;
        } 
        return -1;
    }

    public float RotationCalculator(float staticRoom, float newRoom)
    {
        if (staticRoom > 360.0f)
            staticRoom -= 360.0f;
        if (newRoom > 360.0f)
            newRoom -= 360.0f;
        float ret = 0.0f;
        

        ret = (staticRoom - newRoom - 180.0f);

        if (ret > 360.0f)
            ret -= 360.0f;

        //Debug.Log(ret);
        
        return ret;
    }

    public bool CheckCollision(GameObject _detection, GameObject _ignore = null)
    {
        for (int i = 0; i < GeneratedRooms.Length; i++)
        {
            if (GeneratedRooms[i] != null && GeneratedRooms[i] != _ignore)
            {
                float closestDistance = Vector3.Distance(GeneratedRooms[i].GetComponent<BoxCollider>().ClosestPointOnBounds(_detection.transform.position), GeneratedRooms[i].transform.position);
                if (Vector3.Distance(_detection.GetComponent<BoxCollider>().ClosestPointOnBounds(GeneratedRooms[i].transform.position), GeneratedRooms[i].transform.position) <= closestDistance)
                {
                    Debug.Log("fail");
                    return false;
                }
            }
        }

        for (int i = 0; i < GeneratedHalls.Length; i++)
        {
            if (GeneratedHalls[i] != null && GeneratedHalls[i] != _ignore)
            {
                float closestDistance = Vector3.Distance(GeneratedHalls[i].GetComponent<BoxCollider>().ClosestPointOnBounds(_detection.transform.position), GeneratedHalls[i].transform.position);
                if (Vector3.Distance(_detection.GetComponent<BoxCollider>().ClosestPointOnBounds(GeneratedHalls[i].transform.position), GeneratedHalls[i].transform.position) <= closestDistance)
                {
                    Debug.Log("fail");
                    return false;
                }
            }
        }
        Debug.Log(true);

        return true;
    }
}
