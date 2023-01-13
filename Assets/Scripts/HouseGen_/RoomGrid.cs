using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid 
{
    //Constants for neighbour sides
    const int LEFT = 1;
    const int RIGHT = 2;
    const int DOWN = 3;
    const int UP = 4;

    //Variables for room grid, linked lists and their prefabs
    private RoomNode headRoom;
    private RoomNode[,] roomGrid;
    private GameObject[] prefabRooms;

    //Variables in track of added rooms each round & original starting rooms
    const int ADDED_ROOMS = -10;
    private int activeRooms = 104;

    //Pre:
    //Post:
    //Desc: Constructor for RoomGrid object
    public RoomGrid(GameObject[] prefabRooms)
    {
        this.prefabRooms = prefabRooms;
    }

    //Pre:
    //Post:
    //Desc: Regenerated rooms randomly
    public void RegenerateRooms ()
    {
        //Will only add Added_Rooms when active rooms is greater than or equal to 20
        if(activeRooms >= 20) activeRooms += ADDED_ROOMS;

        //Various variables for regeneration
        headRoom = new RoomNode(new Vector2(activeRooms, activeRooms));
        RoomNode curNode;
        int ranRoom;
        bool roomFound = false;
        int freeSpace = 1;
        int dir;
        int noNodeCount = 0;

        //Used in determining whether a room with two neighbors has opposite, or adjacent ones... ONCounter (opposite neighbor counter)
        bool adj2 = true;
        int ONCounter ;

        //Creating grid, and assigning the head node
        roomGrid = new RoomNode[activeRooms * 2, activeRooms * 2];
        roomGrid[activeRooms, activeRooms] = headRoom;

        //Loop for generating rooms. Starting at 1 as the first room has already been created
        for (int i = 1; i < activeRooms; i++)
        {
            adj2 = true;
            ONCounter = 0;

            //Variables used in searching for availible space
            curNode = headRoom;
            roomFound = false;
            ranRoom = Random.Range(1, i + 1);
            
            //Searches for available space
            for(int r = 0; r < ranRoom; r++)
            {
                int rand = Random.Range(1, 5);
                if (curNode.GetNext(rand) != null) curNode = curNode.GetNext(rand);
                else break;
            }

            //Looping untill free space is found
            while (!roomFound)
            {
                //Looping for each neighbouring side
                for (int y = 1; y < 5; y++)
                {
                    //Determines whether space is free
                    if (curNode.GetNext(y) == null)
                    {
                        //Variables used in allowing program to know free space has been allocated
                        freeSpace = y;
                        roomFound = true;
                        break;
                    }
                    else if (y == 4) { curNode = curNode.GetNext(Random.Range(1, 5)); }
                }
            }

            //Setting the current node to the now created free space
            curNode.SetNext(freeSpace);
            curNode = curNode.GetNext(freeSpace);

            //Determines whether no node should be drawn
            if (Random.Range(1, 5) == 4 && noNodeCount < activeRooms/4) {curNode.MakeInactive();}
            roomGrid[(int)curNode.GetLoc().x, (int)curNode.GetLoc().y] = curNode;


            //Assigning neighbors if grid finds there is a neighbor to curNode (on each side)
            if (roomGrid[(int)curNode.GetLoc().x - 1, (int)curNode.GetLoc().y] != null) {AssignNeighbors(curNode, roomGrid[(int)curNode.GetLoc().x - 1, (int)curNode.GetLoc().y], LEFT);}
            if (roomGrid[(int)curNode.GetLoc().x + 1, (int)curNode.GetLoc().y] != null) {AssignNeighbors(curNode, roomGrid[(int)curNode.GetLoc().x + 1, (int)curNode.GetLoc().y], RIGHT);}
            if (roomGrid[(int)curNode.GetLoc().x, (int)curNode.GetLoc().y + 1] != null) {AssignNeighbors(curNode, roomGrid[(int)curNode.GetLoc().x, (int)curNode.GetLoc().y + 1], UP);}
            if (roomGrid[(int)curNode.GetLoc().x, (int)curNode.GetLoc().y - 1] != null) {AssignNeighbors(curNode, roomGrid[(int)curNode.GetLoc().x, (int)curNode.GetLoc().y - 1], DOWN);}

        }
        
        //Looping through the grid
        for (int r = 0; r < activeRooms*2; r++)
        {
            for(int c = 0; c < activeRooms * 2; c++)
            {
                //should the RoomNode not be null and is active (too needing it to have neighbours)
                if (roomGrid[c, r] != null && roomGrid[c, r].IsActive() && roomGrid[c, r].GetNeighbors() > 0)
                {
                    //Determining neighbour count, index for prefab as well as setting the prefab
                    int neighbors = roomGrid[c, r].GetNeighbors();
                    int prefabIndex = neighbors - 1;
                    if (neighbors == 2 && IsOpposite(roomGrid[c, r])) prefabIndex = prefabRooms.Length - 1;
                    roomGrid[c, r].SetPrefab(prefabRooms[prefabIndex]);

                    //Instantiating the room prefab as GameObject
                    GameObject go;
                    go = GameObject.Instantiate(roomGrid[c, r].GetPrefab()) as GameObject;  
                    go.transform.position = new Vector3(((roomGrid[c, r].GetLoc().x-activeRooms) * 10), 0, ((roomGrid[c, r].GetLoc().y-activeRooms) * 10));
                    roomGrid[c, r].SetPrefab(go);

                    //Calls upon RotateRooms subprogram to correctly allign rooms
                    RotateRoom(neighbors, roomGrid[c, r]);
                }
            }
        }
    }

    //Pre: Valied number of neighbours and RoomNode
    //Post:
    //Desc: returns RoomNode's neighbour count
    private void RotateRoom(int neighbors, RoomNode room)
    {
        //Variables used in determining number of 90 degree rotations
        int freind = 0;
        int numRotations;

        //looping through each side of RoomNode
        for (int i = 1; i <= 4; i++)
        {
            //Determining index at which the reference side sits
            if (neighbors == 3) if (room.GetNext(i) == null || !room.GetNext(i).IsActive())
            {
                freind = oppositeSide(i);
                break;
            }
            if (neighbors == 2 && IsOpposite(room)) 
            {
                if (room.GetNext(i) != null && room.GetNext(i).IsActive())
                {
                    freind = i;
                    break;
                }
            }
            if (neighbors == 2 && !IsOpposite(room))
            {
                freind = AdjFreindFinder(room);
            }
            if (neighbors == 1) if (room.GetNext(i) != null && room.GetNext(i).IsActive())
            {
                freind = i;
                break;
            }
        }

        //Calls upon subprogram to calculate number of necessary rotations
        numRotations = CalcNumRotations(freind);
        for (int i = 0; i < numRotations; i++) 
        {
            //Rotates RoomNode's relative prefab
            room.GetPrefab().transform.rotation *= Quaternion.Euler(0, 90, 0); 
        }
    }

    //Pre:
    //Post:
    //Desc: returns number of needed rotations
    private int CalcNumRotations(int dir)
    {
        switch (dir)
        {
            case LEFT:
                return 0;
            break;

            case RIGHT:
                return 2;
            break;

            case UP:
                return 1;
            break;

            case DOWN:
                return 3;
            break;

            default:
                return -1;
            break;
        }
    }

    //Pre:
    //Post:
    //Desc: returns RoomNode's reference side for 2Door-adjacent prefabs
    private int AdjFreindFinder(RoomNode room)
    {
        //Variables used in determining reference side
        int[] localFreinds = new int[2];
        int freindNum = 0;


        //looping through each side
        for(int i = 1; i <= 4; i++)
        {
            //Should the neighbour not equal nullened and is active
            if(room.GetNext(i) != null && room.GetNext(i).IsActive())
            {
                //Store sides
                localFreinds[freindNum] = i;
                freindNum++;
                if (freindNum > 1) break;
            }
        }

        //Determining reference side
        if (localFreinds[0] == LEFT && localFreinds[1] == DOWN) return LEFT;
        if (localFreinds[0] == RIGHT && localFreinds[1] == DOWN) return DOWN;
        if (localFreinds[0] == LEFT && localFreinds[1] == UP) return UP;
        if (localFreinds[0] == RIGHT && localFreinds[1] == UP) return RIGHT;
        else return -1;
    }

    //Pre:
    //Post:
    //Desc: returns boolean value pertaining as to whether RoomNode with 2 neighbours is opposite or adjacent
    private bool IsOpposite(RoomNode room)
    {
        //Determines whether left room and right are oppsite
        if(room.GetNext(LEFT) != null && room.GetNext(RIGHT) != null)
        {
            if (room.GetNext(LEFT).IsActive() && room.GetNext(RIGHT).IsActive())
            {
                return true;
            }
        }

        //Determines whether upper and lower rooms are opposite
        if (room.GetNext(UP) != null && room.GetNext(DOWN) != null)
        {
            if (room.GetNext(UP).IsActive() && room.GetNext(DOWN).IsActive())
            {
                return true;
            }
        }

        return false;
    }

    //Pre: Valid side
    //Post:
    //Desc: returns the opposite side of parameter
    private int oppositeSide (int dir)
    {
        if (dir == LEFT) return RIGHT;
        else if (dir == RIGHT) return LEFT;
        else if (dir == DOWN) return UP;
        else return DOWN;
    }

    //Pre:
    //Post:
    //Desc: Assigns two rooms to be one another's neighbours
    private void AssignNeighbors(RoomNode room, RoomNode room2, int side)
    {
        room.SetNext(side, room2);
        room2.SetNext(oppositeSide(side), room);
    }

    //Pre:
    //Post:
    //Desc: Returns roomnode determinent on coordinate
    public GameObject GetRoomNode(int x, int y)
    {
        if (roomGrid[x, y] == null || !roomGrid[x, y].IsActive()) return null;
        return roomGrid[x, y].GetPrefab();
    }

    //Pre:
    //Post:
    //Desc: Returns the grid's side length
    public int GetGridLength()
    {
        return activeRooms * 2;
    }

    //Pre:
    //Post: Vector3 of center of prefab room
    //Desc: Searches through available RoomNodes 
    public Vector3 FindSuitableSpawn()
    {
        //Determining number of iterations
        int randRoom = Random.Range(1, activeRooms / 2);
        int randNeighbour;
        RoomNode curNode = headRoom;

        //Looping through iterations
        for(int i = 0; i < randRoom; i++)
        {
            //Setting curNode to a new node
            randNeighbour = Random.Range(1, 5);
            if (curNode.GetNext(randNeighbour) != null && curNode.GetNext(randNeighbour).IsActive()) curNode = curNode.GetNext(randNeighbour);
        }

        //Returns prefab of current node
        return curNode.GetPrefab().transform.position + new Vector3(0, 1, 0);
    }
}
