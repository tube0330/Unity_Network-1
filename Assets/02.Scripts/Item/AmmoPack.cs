using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 30;   //탄환

    public void Use(GameObject target)
    {
        PlayerShooter shooter = target.GetComponent<PlayerShooter>();

        if (shooter != null && shooter.c_gun != null)
            shooter.c_gun.remainAmmo += ammo;

        Debug.Log("탄알 증가" + ammo);
        Destroy(gameObject);
    }
}
