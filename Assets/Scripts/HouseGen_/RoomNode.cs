using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode 
{
    //Variables which are maintained by each RoomNode
    private Vector2 loc;
    private GameObject prefab;
    private int neighbors = 0;
    private int roomWidth = 10;
    private bool active = true;

    private RoomNode[] rooms;

    //Pre: requires a 2 dim. vector
    //Post: returns RoomNode object
    //Desc: Constructor for RoomNode
    public RoomNode(Vector2 loc)
    {
        this.loc = loc;
        rooms = new RoomNode[4];
    }

    //Pre: prefab GameObject is correctly assigned
    //Post: 
    //Desc: Sets the stored prefab of this RoomNode object
    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }

    //Pre:
    //Post:
    //Desc: Sets the current RoomNode object inactive
    public void MakeInactive()
    {
        active = false;
    }

    //Pre:
    //Post:
    //Desc: Returns a boolean value determinent of whether the RoomNode is active
    public bool IsActive()
    {
        return active;
    }

    //Pre:
    //Post:
    //Desc: returns RoomNode's neighbour count
    public int GetNeighbors() { return neighbors; }

    //Pre:
    //Post: Vector2
    //Desc: returns RoomNode's location (on grid)
    public Vector2 GetLoc() { return loc; }

    //Pre:
    //Post:
    //Desc: Sets the location of this RoomNode
    public void SetLoc(Vector2 loc) { this.loc = loc;  }

    //Pre:
    //Post:
    //Desc: returns RoomNode's prefab GameObject
    public GameObject GetPrefab() { return prefab; }

    //Pre: Valid index
    //Post:
    //Desc: returns current RoomNode's neighbour (RoomNode)
    public RoomNode GetNext(int side) {
        return rooms[side - 1]; }

    //Pre: Valied/appropriate index and RoomNode
    //Post:
    //Desc: Sets the next neighbour of this RoomNode
    public void SetNext(int side, RoomNode nextRoom)
    {
        rooms[side - 1] = nextRoom;
        if(nextRoom.IsActive()) neighbors++;
    }

    //Pre: Valied and correct side index
    //Post:
    //Desc: returns RoomNode's neighbour count
    public void SetNext(int side) 
    {
        //Variables in determining new RoomNode 
        RoomNode newRoom;
        Vector2 newLoc = loc;

        //Determines requested side and sequentiate action
        switch (side)
        {
            case 1:
                newLoc.x--;
                break;
            case 2:
                newLoc.x++;
                break;
            case 3:
                newLoc.y--;
                break;
            case 4:
                newLoc.y++;
                break;
        }

        //Creating and referencing new RoomNode
        newRoom = new RoomNode(newLoc);
        rooms[side - 1] = newRoom;
    }
}
