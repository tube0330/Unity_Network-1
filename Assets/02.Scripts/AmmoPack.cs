using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 30;   //탄환

    public void Use(GameObject target)
    {
        Debug.Log("탄알 증가" + ammo);
    }
}
