using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager 
{
    //Creating variables for room prefab and grid
    private GameObject[] rooms;
    private RoomGrid roomGrid;

    //Pre: Prefab rooms are in correct orientation and order
    //Post:
    //Desc: Constructor for RoomManager
    public RoomManager(GameObject[] prefabRooms)
    {
        this.rooms = prefabRooms;
        roomGrid = new RoomGrid(rooms);
    }

    //Pre:
    //Post:
    //Desc: Randomly regenerates rooms
    public void RegenerateRooms()
    {
        roomGrid.RegenerateRooms();
    }

    //Pre:
    //Post:
    //Desc: Returns availible grid position
    public Vector3 GetNewGridPosition()
    {
        return roomGrid.FindSuitableSpawn();
    }

    //Pre:
    //Post:
    //Desc: returns RoomNode from grid location
    public GameObject GetRoomNode(int x, int y)
    {
        return roomGrid.GetRoomNode(x, y);
    }

    //Pre:
    //Post:
    //Desc: returns total grid side length
    public int GetGridLength()
    {
        return roomGrid.GetGridLength();
    }
}
