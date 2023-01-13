using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Various weapon data
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;

    private float curCoolDown;
    private int curIndex;
    private GameObject curWeapon;

    void Update()
    {
        //Equips weapon when "1" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (curWeapon != null)
        {
            //Calls aim subprogram for ADS
            Aim(Input.GetMouseButton(1));

            //Should the shoot button be pressed and no cooldown is present "shoot" subprogram is called
            if (Input.GetMouseButtonDown(0) && curCoolDown <= 0) Shoot();

            //Weapon positional elasticity
            curWeapon.transform.localPosition = Vector3.Lerp(curWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

            //Cool Down
            if (curCoolDown > 0) curCoolDown -= Time.deltaTime;
        }
    }

    //Pre:
    //Post:
    //Desc: Equips a new weapon
    public void Equip(int weapon)
    {
        //Destroys current weapon
        if (curWeapon != null) Destroy(curWeapon);

        //Reasigns currene index
        curIndex = weapon;

        //Instantiates the weapon
        GameObject equipped = Instantiate(loadout[weapon].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        equipped.transform.localPosition = Vector3.zero;
        equipped.transform.localEulerAngles = Vector3.zero;

        //Reassigns the current node
        curWeapon = equipped;
    }

    //Pre: boolean as parameter
    //Post: n/a
    //Desc: Based on previously stored lcoations, updates weapon location to ADS
    void Aim(bool isAiming)
    {
        //Referencing various states
        Transform anchor = curWeapon.transform.Find("Anchor");
        Transform ADSstate = curWeapon.transform.Find("States/ADS");
        Transform hipState = curWeapon.transform.Find("States/Hip");

        //Smoothly transitions to new location (from previous)
        if (isAiming) anchor.position = Vector3.Lerp(anchor.position, ADSstate.position, Time.deltaTime * loadout[curIndex].adsSpeed);
        else anchor.position = Vector3.Lerp(anchor.position, hipState.position, Time.deltaTime * loadout[curIndex].adsSpeed);
    }

    //Pre:
    //post:
    //Desc: Fires Gun, provides all necessary calculations for determining collision info
    void Shoot()
    {
        //Referencing the player's first person camera
        Transform spawn = transform.Find("Cameras/Main Camera");

        //bloom forming
        Vector3 bloom = spawn.position + spawn.forward * 1000f;
        bloom += Random.Range(-loadout[curIndex].bloom, loadout[curIndex].bloom) * spawn.up;
        bloom += Random.Range(-loadout[curIndex].bloom, loadout[curIndex].bloom) * spawn.right;
        bloom -= spawn.position;
        bloom.Normalize();

        //Raycast data and handling
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(spawn.position, bloom, out hit, 1000f, canBeShot))
        {
            //Determines whether a valid hit on enemy has been detected, makes reference of driver class
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) FindObjectOfType<DriverClass>().EnemyShot(loadout[curIndex].damage);
            else
            {
                //Adding 0.001 in the normal direction of hit (away from wall). Removes clone when appropriated
                GameObject newHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                newHole.transform.LookAt(hit.point + hit.normal);
                Destroy(newHole, 5f);
            }
        }

        //Gun Effects of kickback and recoil
        curWeapon.transform.Rotate(-loadout[curIndex].recoil, 0, 0);
        curWeapon.transform.position -= curWeapon.transform.forward * loadout[curIndex].kick;

        //CoolDown
        curCoolDown = loadout[curIndex].fireRate;
    }
}
