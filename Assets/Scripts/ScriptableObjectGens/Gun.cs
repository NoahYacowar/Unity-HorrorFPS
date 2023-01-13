using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

public class Gun : ScriptableObject
{
    //Variables used in characterizing weapons
    public string name;
    public int damage;
    public float fireRate;
    public float bloom;
    public float recoil;
    public float kick;
    public float adsSpeed;
    public GameObject prefab;
}
