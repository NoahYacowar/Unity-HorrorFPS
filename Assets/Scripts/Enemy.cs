using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : SmartEntity
{
    //Storing the current maximum health value
    private int maxHealth;

    //Enemy constructor, passess off required data to 
    public Enemy(Transform player, int health) : base(player, health)
    {
        maxHealth = health;
    }

    //Pre:
    //Post:
    //Desc: Sets the new maximum health value of the player
    public void SetNewMaxHealth(int maxH)
    {
        maxHealth = maxH;
        health = maxHealth;
    }

    //pre:
    //Post: 
    //Desc: returns the maximum health of <Enemy>
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
