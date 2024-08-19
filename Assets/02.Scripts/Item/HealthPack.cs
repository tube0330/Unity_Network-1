using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float health = 50;

    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();
        if (life != null)
            life.RestoreHP(health);
            
        Debug.Log("체력 회복" + health);
        Destroy(gameObject);
    }
}
