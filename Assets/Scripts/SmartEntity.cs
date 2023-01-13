using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEntity
{
    //Variables defining obect characteristics
    protected Transform transform;
    protected int health;
    bool alive = true;

    //Constructor for SmartEntity class
    public SmartEntity(Transform player, int health)
    {   //Assigning variables;
        transform = player;
        this.health = health;
    }

    //Pre: A 3D position stored as vector
    //Post:
    //Desc: Resets the player's position
    public void SetPlayerPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    //Pre: Increment value 
    //Post: 
    //Desc: Applies increment to user's health
    public void IncrementHealth(int increment)
    {
        health += increment;
        if (health <= 0) alive = false;
    }

    //Pre:
    //Post:
    //Desc: Sets the health to 0, sets boolean determinent of living state, to false
    public void Kill()
    {
        health = 0;
        alive = false;
    }

    //Pre:
    //Post:
    //Desc: returns health remaining
    public int GetHealth()
    {
        return health;
    }
}
