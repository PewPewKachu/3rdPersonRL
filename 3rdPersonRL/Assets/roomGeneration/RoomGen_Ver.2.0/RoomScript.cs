using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] Transform[] doors;
    [SerializeField] GameObject[] doorBlocks;
    [SerializeField] bool touching = false;

    private void Start()
    {
        if (doorBlocks.Length == 0)
            doorBlocks = new GameObject[doors.Length];
    }

    public void setDoor(int i)
    {
        if(doorBlocks.Length > i
            && doorBlocks[i] != null)
            Destroy(doorBlocks[i]);
    }

    public Transform[] getDoors()
    {
        return doors;
    }

    public bool isTouching()
    {
        return touching;
    }

    private void OnTriggerEnter(Collider other)
    {
        touching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        touching = false;
    }
}
