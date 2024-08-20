using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HealthPack : MonoBehaviourPun, IItem
{
    public float health = 50;

    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();
        if (life != null)
            life.RestoreHP(health);

        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);
    }
}
